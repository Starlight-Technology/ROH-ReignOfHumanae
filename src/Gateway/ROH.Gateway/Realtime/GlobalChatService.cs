using MessagePack;

using Microsoft.Extensions.Options;

using ROH.Context.Player.Mongo.Entities;
using ROH.Context.Player.Mongo.Interface;
using ROH.Contracts.WebSocket;
using ROH.Contracts.WebSocket.Chat;

using System.Net.WebSockets;

namespace ROH.Gateway.Realtime;

public class GlobalChatService(
    IChatRepository repository,
    IRealtimeSessionRegistry sessionRegistry,
    IOptions<RealtimeOptions> options,
    ILogger<GlobalChatService> logger) : IGlobalChatService
{
    readonly RealtimeOptions _options = options.Value;

    public async Task HandleHistoryRequestAsync(
        RealtimeClientSession session,
        byte[] payload,
        CancellationToken cancellationToken = default)
    {
        ChatHistoryRequest request;
        try
        {
            request = MessagePackSerializer.Deserialize<ChatHistoryRequest>(payload);
        }
        catch (MessagePackSerializationException)
        {
            await SendErrorAsync(
                session,
                "InvalidPayload",
                "A solicitação de histórico é inválida.",
                cancellationToken: cancellationToken);
            return;
        }

        if (request.Channel != ChatChannel.Global)
        {
            await SendErrorAsync(
                session,
                "UnsupportedChannel",
                "Somente o canal Global está disponível.",
                cancellationToken: cancellationToken);
            return;
        }

        int configuredLimit = _options.ChatHistoryLimit > 0 ? _options.ChatHistoryLimit : 50;
        int limit = request.Limit > 0 ? Math.Min(request.Limit, configuredLimit) : configuredLimit;
        IReadOnlyList<ChatMessageEntity> entities = await repository
            .GetRecentAsync(nameof(ChatChannel.Global), limit, cancellationToken)
            .ConfigureAwait(false);

        ChatHistoryResponse response = new()
        {
            Channel = ChatChannel.Global,
            Messages = [.. entities.Select(ToContract)]
        };

        await session.SendAsync(
                new RealtimeEnvelope
                {
                    Type = RealtimeEventTypes.ChatHistoryResponse,
                    Payload = MessagePackSerializer.Serialize(response)
                },
                cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task HandleSendAsync(
        RealtimeClientSession session,
        byte[] payload,
        CancellationToken cancellationToken = default)
    {
        ChatSendMessage request;
        try
        {
            request = MessagePackSerializer.Deserialize<ChatSendMessage>(payload);
        }
        catch (MessagePackSerializationException)
        {
            await SendErrorAsync(
                session,
                "InvalidPayload",
                "A mensagem de chat é inválida.",
                cancellationToken: cancellationToken);
            return;
        }

        ChatValidationResult validation = ChatMessageValidator.Validate(
            request.Channel,
            request.Message,
            _options.ChatMaxMessageLength);

        if (!validation.IsValid)
        {
            await SendErrorAsync(
                session,
                validation.Code,
                validation.Message,
                cancellationToken: cancellationToken);
            return;
        }

        if (!session.ChatRateLimiter.TryAcquire(out int retryAfterMilliseconds))
        {
            await SendErrorAsync(
                session,
                "RateLimited",
                "Você está enviando mensagens rápido demais.",
                retryAfterMilliseconds,
                cancellationToken);
            return;
        }

        ChatMessageEntity entity = new()
        {
            Channel = nameof(ChatChannel.Global),
            CreatedAtUtc = DateTime.UtcNow,
            Message = validation.SanitizedMessage,
            SenderCharacterId = session.CharacterId,
            SenderDisplayName = session.DisplayName,
            Tags = ["global"]
        };

        await repository.InsertAsync(entity, cancellationToken).ConfigureAwait(false);

        RealtimeEnvelope envelope = new()
        {
            Type = RealtimeEventTypes.ChatMessage,
            Payload = MessagePackSerializer.Serialize(ToContract(entity))
        };

        RealtimeClientSession[] recipients = [.. sessionRegistry.Snapshot()];
        await Task.WhenAll(recipients.Select(recipient => SendBroadcastAsync(recipient, envelope, cancellationToken)))
            .ConfigureAwait(false);
    }

    static ChatMessage ToContract(ChatMessageEntity entity) => new()
    {
        Channel = ChatChannel.Global,
        CreatedAtUtc = entity.CreatedAtUtc,
        Id = entity.Id.ToString(),
        Message = entity.Message,
        SenderCharacterId = entity.SenderCharacterId,
        SenderDisplayName = entity.SenderDisplayName,
        Tags = entity.Tags,
        TargetCharacterId = entity.TargetCharacterId
    };

    async Task SendBroadcastAsync(
        RealtimeClientSession recipient,
        RealtimeEnvelope envelope,
        CancellationToken cancellationToken)
    {
        try
        {
            await recipient.SendAsync(envelope, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception exception) when (exception is WebSocketException or OperationCanceledException)
        {
            logger.LogDebug(
                "Could not deliver global chat message to character {CharacterId}: {ErrorType}",
                recipient.CharacterId,
                exception.GetType().Name);
        }
    }

    static Task SendErrorAsync(
        RealtimeClientSession session,
        string code,
        string message,
        int retryAfterMilliseconds = 0,
        CancellationToken cancellationToken = default) =>
        session.SendAsync(
            new RealtimeEnvelope
            {
                Type = RealtimeEventTypes.ChatError,
                Payload = MessagePackSerializer.Serialize(
                    new ChatErrorMessage
                    {
                        Code = code,
                        Message = message,
                        RetryAfterMilliseconds = retryAfterMilliseconds
                    })
            },
            cancellationToken);
}
