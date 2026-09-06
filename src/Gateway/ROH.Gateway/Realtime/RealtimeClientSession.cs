using ROH.Contracts.WebSocket;
using ROH.Service.WebSocket;

using System.Net.WebSockets;

namespace ROH.Gateway.Realtime;

public sealed class RealtimeClientSession
{
    public RealtimeClientSession(
        AuthenticatedRealtimeIdentity identity,
        WebSocket socket,
        RealtimeOptions options)
    {
        AccountId = identity.AccountId;
        CharacterId = identity.CharacterId;
        ChatRateLimiter = new ChatRateLimiter(
            options.ChatRateLimitMessagesPerSecond,
            options.ChatRateLimitBurst);
        ConnectionId = Guid.NewGuid();
        DisplayName = identity.DisplayName;
        InitialTransform = identity.InitialTransform;
        Socket = socket;
        WorldId = identity.WorldId;
    }

    public string AccountId { get; }

    public string CharacterId { get; }

    public ChatRateLimiter ChatRateLimiter { get; }

    public Guid ConnectionId { get; }

    public string DisplayName { get; }

    public RealtimePlayerTransform InitialTransform { get; }

    public WebSocket Socket { get; }

    public string WorldId { get; }

    public async Task CloseAsync(
        WebSocketCloseStatus closeStatus,
        string description,
        CancellationToken cancellationToken = default) =>
        await Socket
            .CloseSerializedAsync(closeStatus, description, cancellationToken)
            .ConfigureAwait(false);

    public Task SendAsync(RealtimeEnvelope envelope, CancellationToken cancellationToken = default) =>
        Socket.SendAsync(envelope, cancellationToken);
}
