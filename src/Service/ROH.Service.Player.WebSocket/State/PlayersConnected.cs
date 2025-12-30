//-----------------------------------------------------------------------
// <copyright file="PlayersConnected.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ROH.Service.Player.WebSocket.State;

using MessagePack;

using ROH.Contracts.GRPC.Player.NearbyPlayer;
using ROH.Contracts.GRPC.Worker.PlayerSocket;
using ROH.Contracts.WebSocket;
using ROH.Contracts.WebSocket.Player;
using ROH.Service.Player.WebSocket.Interface;
using ROH.Service.WebSocket;

using System.Threading.Tasks;

public class PlayersConnected(IPlayerPositionServiceSocket playerPositionService) : PlayerConnectedService.PlayerConnectedServiceBase
{
    public override async Task<Contracts.GRPC.Worker.PlayerSocket.PlayersConnected> GetConnectedPlayers(
        Default request,
        global::Grpc.Core.ServerCallContext context)
    {
        System.Collections.Concurrent.ConcurrentDictionary<string, System.Net.WebSockets.WebSocket> connectedPlayers = await playerPositionService.GetPlayersClient(
            );

        Google.Protobuf.Collections.RepeatedField<PlayerConnected> response = new Google.Protobuf.Collections.RepeatedField<PlayerConnected>(
            );

        foreach (KeyValuePair<string, System.Net.WebSockets.WebSocket> player in connectedPlayers)
        {
            response.Add(new PlayerConnected { Id = player.Key });
        }

        return new Contracts.GRPC.Worker.PlayerSocket.PlayersConnected { PlayersId = { response } };
    }

    public override async Task<Default> SendNearbyPlayers(
        NearbyPlayersResponse response,
        global::Grpc.Core.ServerCallContext context)
    {
        System.Collections.Concurrent.ConcurrentDictionary<string, System.Net.WebSockets.WebSocket> connectedPlayers = await playerPositionService.GetPlayersClient(
            );

        if (connectedPlayers.TryGetValue(response.MainPlayer, out System.Net.WebSockets.WebSocket? socket))
        {
            NearbyPlayersMessage message = new NearbyPlayersMessage
            {
                Players =
                    [.. response.Players
                        .Select(
                            p => new NearbyPlayerMessage
                        {
                            PlayerId = p.PlayerId,
                            X = p.X,
                            Y = p.Y,
                            Z = p.Z,
                            RotX = p.RotX,
                            RotY = p.RotY,
                            RotZ = p.RotZ,
                            RotW = p.RotW,
                            ModelName = p.ModelName,
                            AnimationState = (int)p.AnimationState
                        })]
            };

            await WebSocketService.SendAsync(
                socket,
                new RealtimeEnvelope
                {
                    Type = RealtimeEventTypes.GetNearbyPlayers,
                    Payload = MessagePackSerializer.Serialize(message)
                });
        }

        return new Default { A = true };
    }
}