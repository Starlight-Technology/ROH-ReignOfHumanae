//-----------------------------------------------------------------------
// <copyright file="WebSocket.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.Connection
{
    public class WebSocket
    {
        private readonly ClientWebSocket _webSocket;

        public WebSocket()
        { _webSocket = new(); }

        public async Task CloseAsync(
            WebSocketCloseStatus closeStatus,
            string statusDescription,
            CancellationToken cancellationToken)
        { await _webSocket.CloseAsync(closeStatus, statusDescription, cancellationToken).ConfigureAwait(true); }

        /// <summary>
        /// Connect to WebSocket
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task ConnectAsync(Uri uri, CancellationToken cancellationToken)
        { await _webSocket.ConnectAsync(uri, cancellationToken).ConfigureAwait(true); }

        public async Task SendJsonAsync(object payload, CancellationToken cancellationToken)
        {
            string json = JsonUtility.ToJson(payload);
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            await _webSocket.SendAsync(
                new ArraySegment<byte>(buffer),
                WebSocketMessageType.Text,
                true,
                cancellationToken).ConfigureAwait(true);
        }
    }
}