using ROH.Gateway.Realtime;

namespace ROH.Test.Realtime;

public class ChatCleanupServiceTests
{
    [Fact]
    public void CalculateCutoffUtc_ShouldUseConfiguredRetention()
    {
        DateTime nowUtc = new(2026, 6, 10, 12, 0, 0, DateTimeKind.Utc);

        DateTime cutoffUtc = ChatCleanupService.CalculateCutoffUtc(nowUtc, 24);

        Assert.Equal(nowUtc.AddHours(-24), cutoffUtc);
    }

    [Fact]
    public void CalculateCutoffUtc_ShouldEnforceOneHourMinimum()
    {
        DateTime nowUtc = new(2026, 6, 10, 12, 0, 0, DateTimeKind.Utc);

        DateTime cutoffUtc = ChatCleanupService.CalculateCutoffUtc(nowUtc, 0);

        Assert.Equal(nowUtc.AddHours(-1), cutoffUtc);
    }
}
