namespace ROH.Gateway.Realtime;

public interface IGlobalChatService
{
    Task HandleHistoryRequestAsync(
        RealtimeClientSession session,
        byte[] payload,
        CancellationToken cancellationToken = default);

    Task HandleSendAsync(
        RealtimeClientSession session,
        byte[] payload,
        CancellationToken cancellationToken = default);
}
