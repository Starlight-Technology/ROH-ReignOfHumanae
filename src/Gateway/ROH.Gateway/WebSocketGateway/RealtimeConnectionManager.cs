using Grpc.Net.Client;

using MessagePack;

using Microsoft.IdentityModel.Tokens;

using ROH.Contracts.GRPC.Player.PlayerPosition;
using ROH.Contracts.WebSocket;
using ROH.Contracts.WebSocket.Player;
using ROH.Service.Exception.Interface;
using ROH.Utils.ApiConfiguration;

using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;

using static ROH.Utils.ApiConfiguration.ApiConfigReader;

namespace ROH.Gateway.WebSocketGateway;

public class RealtimeConnectionManager(
    IExceptionHandler exceptionHandler)
: IRealtimeConnectionManager
{
    private readonly ConcurrentDictionary<string, WebSocket> _accountConnections = new();
    private readonly ConcurrentDictionary<string, WebSocket> _playerConnections = new();

    private Contracts.GRPC.Player.PlayerPosition.PlayerService.PlayerServiceClient _savePlayerPositionApi;

    string accountGuid = string.Empty;

    public async Task<ConcurrentDictionary<string, WebSocket>> GetPlayersClient() => _playerConnections;

    public async Task HandleClientAsync(HttpContext ctx, WebSocket socket)
    {


        try
        {
            accountGuid = Authenticate(ctx);
        }
        catch (Exception e)
        {
            await socket.CloseAsync(
                WebSocketCloseStatus.PolicyViolation,
                "Unauthorized",
                CancellationToken.None);

            exceptionHandler.HandleException(e);

            return;
        }

        _accountConnections[accountGuid] = socket;

        var buffer = new byte[8192];

        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer, CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                    break;

                var data = buffer.AsMemory(0, result.Count);

                var options = MessagePackSerializerOptions.Standard;
                var envelope = MessagePackSerializer.Deserialize<RealtimeEnvelope>(data, options);

                await HandleMessage(envelope, socket);
            }
        }
        catch (WebSocketException e)
        {
            Console.WriteLine(e.WebSocketErrorCode);
            exceptionHandler.HandleException(e);
        }
        catch (Exception e)
        {
            exceptionHandler.HandleException(e);
        }
        finally
        {
            _accountConnections.TryRemove(accountGuid, out _);

            if (socket.State == WebSocketState.Open)
            {
                await socket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "Disconnected",
                    CancellationToken.None);
            }
        }
    }

    private async Task HandleMessage(RealtimeEnvelope env, WebSocket socket)
    {
        switch (env.Type)
        {
            case "PlayerPosition":
                await HandlePlayerPosition(env.Payload, socket);
                break;
        }
    }

    private async Task HandlePlayerPosition(byte[] payload, WebSocket socket)
    {
        var msg = MessagePackSerializer.Deserialize<PlayerPositionMessage>(payload);

        _playerConnections[msg.PlayerId] = socket;

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

        _savePlayerPositionApi = new(channel);

        await _savePlayerPositionApi.SavePlayerDataAsync(
            new PlayerRequest
            {
                PlayerId = msg.PlayerId,
                Position = new Position
                {
                    X = msg.X,
                    Y = msg.Y,
                    Z = msg.Z
                },
                Rotation = new Rotation
                {
                    X = msg.RotX,
                    Y = msg.RotY,
                    Z = msg.RotZ,
                    W = msg.RotW
                },
                AnimationSate = (uint)msg.AnimationState
            }
        );
    }

    private static string Authenticate(HttpContext ctx)
    {
        if (!ctx.Request.Query.TryGetValue("access_token", out var tokenValue))
            throw new UnauthorizedAccessException("WebSocket sem token JWT.");

        string token = tokenValue!;

        string keyToken =
            Environment.GetEnvironmentVariable("ROH_KEY_TOKEN")
            ?? "thisisaverysecurekeywith32charslong!";

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(keyToken);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),

            ValidateIssuer = true,
            ValidIssuer = "ROH.Services.Authentication.AuthService",

            ValidateAudience = true,
            ValidAudience = "ROH.Gateway",

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30)
        };

        ClaimsPrincipal principal;

        try
        {
            principal = tokenHandler.ValidateToken(
                token,
                validationParameters,
                out _);
        }
        catch
        {
            throw new UnauthorizedAccessException("JWT inválido.");
        }

        // 🔑 playerId vem do JTI
        string? accountGuid = principal.Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)
            ?.Value;

        if (string.IsNullOrWhiteSpace(accountGuid))
            throw new UnauthorizedAccessException("JWT sem PlayerId.");

        return accountGuid;
    }
}
