//-----------------------------------------------------------------------
// <copyright file="IPlayerPositionServiceSocket.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.Contracts.GRPC.Player.PlayerPosition;

using System.Collections.Concurrent;

namespace ROH.Service.Player.WebSocket.Interface;

public interface IPlayerPositionServiceSocket
{
    Task<ConcurrentDictionary<string, System.Net.WebSockets.WebSocket>> GetPlayersClient();

    Task<SaveResponse> FlushPlayerPosition(
        string characterId,
        CancellationToken cancellationToken = default);

    Task<SaveResponse> HandlePlayerPosition(
        byte[] payload,
        string characterId,
        string accountId,
        string worldId,
        System.Net.WebSockets.WebSocket socket,
        CancellationToken cancellationToken = default);

    Task RegisterPlayerClient(string characterId, System.Net.WebSockets.WebSocket socket);

    Task RemovePlayerClient(
        string characterId,
        System.Net.WebSockets.WebSocket socket,
        CancellationToken cancellationToken = default);
}
