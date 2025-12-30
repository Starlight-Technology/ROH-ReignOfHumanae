//-----------------------------------------------------------------------
// <copyright file="RealtimeConnectionManager.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using MessagePack;

using Microsoft.IdentityModel.Tokens;

using ROH.Contracts.WebSocket;
using ROH.Service.Exception.Interface;
using ROH.Service.Player.WebSocket.Interface;

using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using ROH.Service.WebSocket;
using ROH;
using ROH.Service;

namespace ROH.Gateway.Controllers.Websocket;

public class RealtimeConnectionManager(
    IExceptionHandler exceptionHandler,
    IPlayerPositionServiceSocket playerPositionService) : IRealtimeConnectionManager
{
    readonly ConcurrentDictionary<string, WebSocket> _accountConnections = new();
    string accountGuid = string.Empty;

    static string Authenticate(HttpContext ctx)
    {
        if (!ctx.Request.Query.TryGetValue("access_token", out Microsoft.Extensions.Primitives.StringValues tokenValue))
            throw new UnauthorizedAccessException("WebSocket sem token JWT.");

        string token = tokenValue!;

        string keyToken =
            Environment.GetEnvironmentVariable("ROH_KEY_TOKEN") ?? "thisisaverysecurekeywith32charslong!";

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.ASCII.GetBytes(keyToken);

        TokenValidationParameters validationParameters = new TokenValidationParameters
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
            principal = tokenHandler.ValidateToken(token, validationParameters, out _);
        }
        catch
        {
            throw new UnauthorizedAccessException("JWT inválido.");
        }

        string? accountGuid = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)
            ?.Value;

        if (string.IsNullOrWhiteSpace(accountGuid))
            throw new UnauthorizedAccessException("JWT sem PlayerId.");

        return accountGuid;
    }

    async Task HandleMessage(RealtimeEnvelope env, WebSocket socket)
    {
        switch (env.Type)
        {
            case RealtimeEventTypes.SavePlayerPosition:
                await playerPositionService.HandlePlayerPosition(env.Payload, socket);
                break;
        }
    }

    public async Task HandleClientAsync(HttpContext ctx, WebSocket socket)
    {
        try
        {
            accountGuid = Authenticate(ctx);
        }
        catch (Exception e)
        {
            await socket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "Unauthorized", CancellationToken.None);

            exceptionHandler.HandleException(e);

            return;
        }

        _accountConnections[accountGuid] = socket;

        byte[] buffer = new byte[8192];

        try
        {
            while (socket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                    break;

                Memory<byte> data = buffer.AsMemory(0, result.Count);

                MessagePackSerializerOptions options = MessagePackSerializerOptions.Standard;
                RealtimeEnvelope envelope = MessagePackSerializer.Deserialize<RealtimeEnvelope>(data, options);

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
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Disconnected", CancellationToken.None);
            }
        }
    }
}