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

    Task<SaveResponse> HandlePlayerPosition(byte[] payload, System.Net.WebSockets.WebSocket socket);

    Task NewPlayerClient(string guid, System.Net.WebSockets.WebSocket socket);
}