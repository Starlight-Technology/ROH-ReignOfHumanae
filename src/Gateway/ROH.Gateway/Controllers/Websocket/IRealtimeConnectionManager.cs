using ROH.Service.WebSocket;
using ROH;
using ROH.Service;

//-----------------------------------------------------------------------
// <copyright file="IRealtimeConnectionManager.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ROH.Gateway.Controllers.Websocket;

public interface IRealtimeConnectionManager
{
    Task HandleClientAsync(HttpContext ctx, System.Net.WebSockets.WebSocket socket);
}