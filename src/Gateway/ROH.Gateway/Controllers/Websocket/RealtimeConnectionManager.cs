//-----------------------------------------------------------------------
// <copyright file="RealtimeConnectionManager.cs" company="Starlight-Technology">
//     Author:
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using MessagePack;

using Microsoft.Extensions.Options;

using ROH.Context.Player.Mongo.Entities;
using ROH.Context.Player.Mongo.Interface;
using ROH.Contracts.WebSocket;
using ROH.Contracts.WebSocket.Player;
using ROH.Contracts.WebSocket.Session;
using ROH.Gateway.Realtime;
using ROH.Service.Exception.Interface;
using ROH.Service.Player.WebSocket.Interface;
using ROH.Service.WebSocket;

using System.Net.WebSockets;

namespace ROH.Gateway.Controllers.Websocket;

public class RealtimeConnectionManager(
    IExceptionHandler exceptionHandler,
    IPlayerPositionServiceSocket playerPositionService,
    IRealtimeIdentityService identityService,
    IRealtimeSessionRegistry sessionRegistry,
    IPositionRepository positionRepository,
    IGlobalChatService chatService,
    IOptions<RealtimeOptions> options,
    ILogger<RealtimeConnectionManager> logger) : IRealtimeConnectionManager
{
    const int MaximumMessageBytes = 64 * 1024;
    readonly RealtimeOptions _options = options.Value;
    readonly SemaphoreSlim _sessionReplacementGate = new(1, 1);

    public async Task HandleClientAsync(HttpContext context, WebSocket socket)
    {
        RealtimeClientSession? session = null;

        try
        {
            AuthenticatedRealtimeIdentity identity = await identityService
                .AuthenticateAsync(context, context.RequestAborted)
                .ConfigureAwait(false);

            session = new RealtimeClientSession(identity, socket, _options);
            RealtimeClientSession? previousSession;

            await _sessionReplacementGate.WaitAsync(context.RequestAborted).ConfigureAwait(false);
            try
            {
                previousSession = sessionRegistry.Replace(session);
                await playerPositionService.RegisterPlayerClient(session.CharacterId, socket).ConfigureAwait(false);
            }
            finally
            {
                _sessionReplacementGate.Release();
            }

            if (previousSession is not null)
                await ReplacePreviousSessionAsync(previousSession, session.CharacterId).ConfigureAwait(false);

            await SendInitialPositionAsync(session, context.RequestAborted).ConfigureAwait(false);
            await ReceiveLoopAsync(session, context.RequestAborted).ConfigureAwait(false);
        }
        catch (UnauthorizedAccessException exception)
        {
            logger.LogWarning("Rejected realtime connection: {Reason}", exception.Message);
            await CloseSocketSafelyAsync(
                    socket,
                    WebSocketCloseStatus.PolicyViolation,
                    "Unauthorized",
                    CancellationToken.None)
                .ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
        }
        catch (WebSocketException exception)
        {
            logger.LogDebug(
                "Realtime socket ended with {WebSocketErrorCode}.",
                exception.WebSocketErrorCode);
        }
        catch (Exception exception)
        {
            exceptionHandler.HandleException(exception);
        }
        finally
        {
            if (session is not null && sessionRegistry.Remove(session))
            {
                await FlushPositionSafelyAsync(session.CharacterId).ConfigureAwait(false);
                await RemovePlayerSafelyAsync(session).ConfigureAwait(false);
            }

            await CloseSocketSafelyAsync(
                    socket,
                    WebSocketCloseStatus.NormalClosure,
                    "Disconnected",
                    CancellationToken.None)
                .ConfigureAwait(false);
        }
    }

    async Task HandleMessageAsync(
        RealtimeClientSession session,
        RealtimeEnvelope envelope,
        CancellationToken cancellationToken)
    {
        if (!sessionRegistry.IsCurrent(session))
            return;

        switch (envelope.Type)
        {
            case RealtimeEventTypes.SavePlayerPosition:
                await playerPositionService
                    .HandlePlayerPosition(
                        envelope.Payload,
                        session.CharacterId,
                        session.AccountId,
                        session.WorldId,
                        session.Socket,
                        cancellationToken)
                    .ConfigureAwait(false);
                break;
            case RealtimeEventTypes.ChatSend:
                await chatService.HandleSendAsync(session, envelope.Payload, cancellationToken).ConfigureAwait(false);
                break;
            case RealtimeEventTypes.ChatHistoryRequest:
                await chatService
                    .HandleHistoryRequestAsync(session, envelope.Payload, cancellationToken)
                    .ConfigureAwait(false);
                break;
        }
    }

    async Task ReceiveLoopAsync(RealtimeClientSession session, CancellationToken cancellationToken)
    {
        while (session.Socket.State == WebSocketState.Open && sessionRegistry.IsCurrent(session))
        {
            byte[]? message = await ReceiveMessageAsync(session.Socket, cancellationToken).ConfigureAwait(false);
            if (message is null)
                return;

            RealtimeEnvelope envelope = MessagePackSerializer.Deserialize<RealtimeEnvelope>(message);
            await HandleMessageAsync(session, envelope, cancellationToken).ConfigureAwait(false);
        }
    }

    async Task ReplacePreviousSessionAsync(RealtimeClientSession previousSession, string characterId)
    {
        await FlushPositionSafelyAsync(characterId).ConfigureAwait(false);

        using CancellationTokenSource timeout = new(TimeSpan.FromSeconds(2));
        try
        {
            await previousSession.SendAsync(
                    new RealtimeEnvelope
                    {
                        Type = RealtimeEventTypes.DisconnectNotice,
                        Payload = MessagePackSerializer.Serialize(
                            new DisconnectNoticeMessage
                            {
                                Message = "Este personagem foi conectado em outra sessão.",
                                Reason = "DuplicateLogin"
                            })
                    },
                    timeout.Token)
                .ConfigureAwait(false);

            await previousSession
                .CloseAsync(WebSocketCloseStatus.PolicyViolation, "DuplicateLogin", timeout.Token)
                .ConfigureAwait(false);
        }
        catch (Exception exception) when (exception is WebSocketException or OperationCanceledException)
        {
            previousSession.Socket.Abort();
        }
    }

    async Task SendInitialPositionAsync(
        RealtimeClientSession session,
        CancellationToken cancellationToken)
    {
        PlayerPosition? persistedPosition = null;
        try
        {
            persistedPosition = await positionRepository
                .GetPlayerPositionAsync(session.CharacterId, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "Could not load persisted position for character {CharacterId}.",
                session.CharacterId);
        }

        bool restored = persistedPosition is not null;
        RealtimePlayerTransform transform = restored
            ? new RealtimePlayerTransform(
                persistedPosition!.PositionX,
                persistedPosition.PositionY,
                persistedPosition.PositionZ,
                persistedPosition.RotationX,
                persistedPosition.RotationY,
                persistedPosition.RotationZ,
                persistedPosition.RotationW)
            : session.InitialTransform;

        InitialPlayerStateMessage message = new()
        {
            CharacterId = session.CharacterId,
            RestoredFromPersistence = restored,
            WorldId = restored && !string.IsNullOrWhiteSpace(persistedPosition!.WorldId)
                ? persistedPosition.WorldId
                : session.WorldId,
            X = transform.X,
            Y = transform.Y,
            Z = transform.Z,
            RotX = transform.RotationX,
            RotY = transform.RotationY,
            RotZ = transform.RotationZ,
            RotW = transform.RotationW
        };

        await session.SendAsync(
                new RealtimeEnvelope
                {
                    Type = RealtimeEventTypes.InitialPlayerState,
                    Payload = MessagePackSerializer.Serialize(message)
                },
                cancellationToken)
            .ConfigureAwait(false);
    }

    async Task FlushPositionSafelyAsync(string characterId)
    {
        try
        {
            await playerPositionService
                .FlushPlayerPosition(characterId, CancellationToken.None)
                .ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "Could not flush the last position for character {CharacterId}.",
                characterId);
        }
    }

    async Task RemovePlayerSafelyAsync(RealtimeClientSession session)
    {
        try
        {
            await playerPositionService
                .RemovePlayerClient(session.CharacterId, session.Socket, CancellationToken.None)
                .ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "Could not remove realtime state for character {CharacterId}.",
                session.CharacterId);
        }
    }

    static async Task<byte[]?> ReceiveMessageAsync(
        WebSocket socket,
        CancellationToken cancellationToken)
    {
        byte[] buffer = new byte[8192];
        using MemoryStream stream = new();

        while (true)
        {
            WebSocketReceiveResult result = await socket
                .ReceiveAsync(buffer, cancellationToken)
                .ConfigureAwait(false);

            if (result.MessageType == WebSocketMessageType.Close)
                return null;

            if (result.MessageType != WebSocketMessageType.Binary)
                throw new WebSocketException(WebSocketError.InvalidMessageType);

            await stream.WriteAsync(buffer.AsMemory(0, result.Count), cancellationToken).ConfigureAwait(false);
            if (stream.Length > MaximumMessageBytes)
                throw new WebSocketException(WebSocketError.HeaderError);

            if (result.EndOfMessage)
                return stream.ToArray();
        }
    }

    static async Task CloseSocketSafelyAsync(
        WebSocket socket,
        WebSocketCloseStatus closeStatus,
        string description,
        CancellationToken cancellationToken)
    {
        try
        {
            if (socket.State is WebSocketState.Open or WebSocketState.CloseReceived)
            {
                await socket
                    .CloseSerializedAsync(closeStatus, description, cancellationToken)
                    .ConfigureAwait(false);
            }
        }
        catch (WebSocketException)
        {
        }
    }
}
