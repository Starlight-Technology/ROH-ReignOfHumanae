//-----------------------------------------------------------------------
// <copyright file="WebSocketService.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using MessagePack;

using ROH.Contracts.WebSocket;

using System.Net.WebSockets;
using System.Runtime.CompilerServices;

namespace ROH.Service.WebSocket;

public static class WebSocketService
{
    private static readonly ConditionalWeakTable<System.Net.WebSockets.WebSocket, SemaphoreSlim> SendLocks = [];

    public static async Task CloseSerializedAsync(
        this System.Net.WebSockets.WebSocket socket,
        WebSocketCloseStatus closeStatus,
        string description,
        CancellationToken cancellationToken = default)
    {
        SemaphoreSlim sendLock = SendLocks.GetValue(socket, _ => new SemaphoreSlim(1, 1));
        await sendLock.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (socket.State is WebSocketState.Open or WebSocketState.CloseReceived)
            {
                await socket
                    .CloseAsync(closeStatus, description, cancellationToken)
                    .ConfigureAwait(false);
            }
        }
        finally
        {
            sendLock.Release();
        }
    }

    public static async Task SendAsync(
        this System.Net.WebSockets.WebSocket socket,
        RealtimeEnvelope env,
        CancellationToken cancellationToken = default)
    {
        byte[] data = MessagePackSerializer.Serialize(env);

        SemaphoreSlim sendLock = SendLocks.GetValue(socket, _ => new SemaphoreSlim(1, 1));
        await sendLock.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (socket.State == WebSocketState.Open)
            {
                await socket
                    .SendAsync(data, WebSocketMessageType.Binary, true, cancellationToken)
                    .ConfigureAwait(false);
            }
        }
        finally
        {
            sendLock.Release();
        }
    }
}
