namespace ROH.Gateway.Realtime;

public interface IRealtimeSessionRegistry
{
    bool IsCurrent(RealtimeClientSession session);

    bool Remove(RealtimeClientSession session);

    RealtimeClientSession? Replace(RealtimeClientSession session);

    IReadOnlyList<RealtimeClientSession> Snapshot();
}
