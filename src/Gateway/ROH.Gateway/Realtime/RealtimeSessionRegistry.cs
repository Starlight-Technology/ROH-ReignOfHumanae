namespace ROH.Gateway.Realtime;

public class RealtimeSessionRegistry : IRealtimeSessionRegistry
{
    private readonly Dictionary<string, RealtimeClientSession> _sessions = new(StringComparer.Ordinal);
    private readonly object _sync = new();

    public bool IsCurrent(RealtimeClientSession session)
    {
        lock (_sync)
        {
            return _sessions.TryGetValue(session.CharacterId, out RealtimeClientSession? active)
                && active.ConnectionId == session.ConnectionId;
        }
    }

    public bool Remove(RealtimeClientSession session)
    {
        lock (_sync)
        {
            return _sessions.TryGetValue(session.CharacterId, out RealtimeClientSession? active)
                && active.ConnectionId == session.ConnectionId && _sessions.Remove(session.CharacterId);
        }
    }

    public RealtimeClientSession? Replace(RealtimeClientSession session)
    {
        lock (_sync)
        {
            _sessions.TryGetValue(session.CharacterId, out RealtimeClientSession? previous);
            _sessions[session.CharacterId] = session;
            return previous;
        }
    }

    public IReadOnlyList<RealtimeClientSession> Snapshot()
    {
        lock (_sync)
        {
            return [.. _sessions.Values];
        }
    }
}
