using Moq;

using ROH.Gateway.Realtime;

using System.Net.WebSockets;

namespace ROH.Test.Realtime;

public class RealtimeSessionRegistryTests
{
    [Fact]
    public void Remove_ShouldNotRemoveReplacementSession()
    {
        RealtimeSessionRegistry registry = new();
        RealtimeClientSession oldSession = CreateSession("character-1");
        RealtimeClientSession newSession = CreateSession("character-1");

        Assert.Null(registry.Replace(oldSession));
        Assert.Same(oldSession, registry.Replace(newSession));

        Assert.False(registry.Remove(oldSession));
        Assert.True(registry.IsCurrent(newSession));
        Assert.Single(registry.Snapshot());
        Assert.True(registry.Remove(newSession));
    }

    private static RealtimeClientSession CreateSession(string characterId)
    {
        AuthenticatedRealtimeIdentity identity = new(
            "account-1",
            characterId,
            "Player",
            "world",
            new RealtimePlayerTransform(0, 0, 0, 0, 0, 0, 1));

        return new RealtimeClientSession(identity, new Mock<WebSocket>().Object, new RealtimeOptions());
    }
}
