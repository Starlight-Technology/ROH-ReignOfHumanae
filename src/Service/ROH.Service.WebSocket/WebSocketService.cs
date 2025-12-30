//-----------------------------------------------------------------------
// <copyright file="WebSocketService.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using MessagePack;

using ROH.Contracts.WebSocket;

using System.Net.WebSockets;

namespace ROH.Service.WebSocket;

public static class WebSocketService
{
    public static async Task SendAsync(this System.Net.WebSockets.WebSocket socket, RealtimeEnvelope env)
    {
        if (socket.State != WebSocketState.Open)
            return;

        byte[] data = MessagePackSerializer.Serialize(env);

        await socket.SendAsync(data, WebSocketMessageType.Binary, true, CancellationToken.None);
    }
}