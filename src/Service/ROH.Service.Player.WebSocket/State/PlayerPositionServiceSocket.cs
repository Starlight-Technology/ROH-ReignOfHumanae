//-----------------------------------------------------------------------
// <copyright file="PlayerPositionServiceSocket.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Grpc.Net.Client;

using MessagePack;

using ROH.Contracts.GRPC.Player.PlayerPosition;
using ROH.Contracts.WebSocket.Player;
using ROH.Service.Player.WebSocket.Interface;
using ROH.Utils.ApiConfiguration;

using System.Collections.Concurrent;

using static ROH.Utils.ApiConfiguration.ApiConfigReader;

namespace ROH.Service.Player.WebSocket.State;

public class PlayerPositionServiceSocket(TimeSpan persistenceInterval) : IPlayerPositionServiceSocket
{
    readonly ConcurrentDictionary<string, DateTime> _lastPersistedAtUtc = new();
    readonly ConcurrentDictionary<string, PlayerRequest> _latestPositions = new();
    readonly ConcurrentDictionary<string, System.Net.WebSockets.WebSocket> _playerConnections = new();
    readonly Lazy<PlayerService.PlayerServiceClient> _savePlayerPositionApi = new(CreateClient);

    public Task<ConcurrentDictionary<string, System.Net.WebSockets.WebSocket>> GetPlayersClient() =>
        Task.FromResult(_playerConnections);

    public async Task<SaveResponse> FlushPlayerPosition(
        string characterId,
        CancellationToken cancellationToken = default)
    {
        if (!_latestPositions.TryGetValue(characterId, out PlayerRequest? latest))
            return new SaveResponse { Success = true };

        PlayerRequest request = CloneRequest(latest, persistPosition: true);
        SaveResponse response = await _savePlayerPositionApi.Value
            .SavePlayerDataAsync(request, cancellationToken: cancellationToken)
            .ResponseAsync
            .ConfigureAwait(false);

        if (response.Success)
            _lastPersistedAtUtc[characterId] = DateTime.UtcNow;

        return response;
    }

    public async Task<SaveResponse> HandlePlayerPosition(
        byte[] payload,
        string characterId,
        string accountId,
        string worldId,
        System.Net.WebSockets.WebSocket socket,
        CancellationToken cancellationToken = default)
    {
        PlayerPositionMessage msg = MessagePackSerializer.Deserialize<PlayerPositionMessage>(payload);

        if (!_playerConnections.TryGetValue(characterId, out System.Net.WebSockets.WebSocket? activeSocket)
            || !ReferenceEquals(activeSocket, socket))
        {
            return new SaveResponse { Success = false };
        }

        DateTime nowUtc = DateTime.UtcNow;
        bool persistPosition = !_lastPersistedAtUtc.TryGetValue(characterId, out DateTime lastPersistedAtUtc)
            || nowUtc - lastPersistedAtUtc >= persistenceInterval;

        PlayerRequest request = new PlayerRequest
        {
            AccountId = accountId,
            AnimationSate = (uint)Math.Max(0, msg.AnimationState),
            PersistPosition = persistPosition,
            PlayerId = characterId,
            Position = new Position { X = msg.X, Y = msg.Y, Z = msg.Z },
            Rotation = new Rotation { X = msg.RotX, Y = msg.RotY, Z = msg.RotZ, W = msg.RotW },
            WorldId = worldId
        };

        SaveResponse response = await _savePlayerPositionApi.Value
            .SavePlayerDataAsync(request, cancellationToken: cancellationToken)
            .ResponseAsync
            .ConfigureAwait(false);

        if (response.Success)
        {
            _latestPositions[characterId] = CloneRequest(request, persistPosition: false);
            if (persistPosition)
                _lastPersistedAtUtc[characterId] = nowUtc;
        }

        return response;
    }

    public Task RegisterPlayerClient(string characterId, System.Net.WebSockets.WebSocket socket)
    {
        _playerConnections[characterId] = socket;
        return Task.CompletedTask;
    }

    public async Task RemovePlayerClient(
        string characterId,
        System.Net.WebSockets.WebSocket socket,
        CancellationToken cancellationToken = default)
    {
        if (!_playerConnections.TryGetValue(characterId, out System.Net.WebSockets.WebSocket? activeSocket)
            || !ReferenceEquals(activeSocket, socket))
        {
            return;
        }

        _playerConnections.TryRemove(
            new KeyValuePair<string, System.Net.WebSockets.WebSocket>(characterId, activeSocket));
        _latestPositions.TryRemove(characterId, out _);
        _lastPersistedAtUtc.TryRemove(characterId, out _);

        await _savePlayerPositionApi.Value
            .RemovePlayerDataAsync(
                new RemovePlayerRequest { PlayerId = characterId },
                cancellationToken: cancellationToken)
            .ResponseAsync
            .ConfigureAwait(false);
    }

    static PlayerRequest CloneRequest(PlayerRequest source, bool persistPosition) => new()
    {
        AccountId = source.AccountId,
        AnimationSate = source.AnimationSate,
        PersistPosition = persistPosition,
        PlayerId = source.PlayerId,
        Position = new Position { X = source.Position.X, Y = source.Position.Y, Z = source.Position.Z },
        Rotation = new Rotation
        {
            W = source.Rotation.W,
            X = source.Rotation.X,
            Y = source.Rotation.Y,
            Z = source.Rotation.Z
        },
        WorldId = source.WorldId
    };

    static PlayerService.PlayerServiceClient CreateClient()
    {
        ApiConfigReader _apiConfig = new();
        Dictionary<ApiUrl, Uri> _apiUrl = _apiConfig.GetApiUrl();
        GrpcChannel channel = GrpcChannel.ForAddress(
            _apiUrl.GetValueOrDefault(ApiUrl.PlayerState) ?? new Uri(string.Empty),
            new GrpcChannelOptions
            {
                HttpHandler =
                    new HttpClientHandler
                        {
                            ServerCertificateCustomValidationCallback =
                                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                        }
            });

        return new PlayerService.PlayerServiceClient(channel);
    }
}
