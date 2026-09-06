using ROH.Gateway.Realtime;

namespace ROH.Test.Realtime;

public class ChatRateLimiterTests
{
    [Fact]
    public void TryAcquire_ShouldRejectUntilTokenIsRefilled()
    {
        DateTime nowUtc = new(2026, 6, 10, 12, 0, 0, DateTimeKind.Utc);
        ChatRateLimiter limiter = new(1, 1, () => nowUtc);

        Assert.True(limiter.TryAcquire(out int firstRetryAfterMilliseconds));
        Assert.Equal(0, firstRetryAfterMilliseconds);
        Assert.False(limiter.TryAcquire(out int retryAfterMilliseconds));
        Assert.Equal(1000, retryAfterMilliseconds);

        nowUtc = nowUtc.AddSeconds(1);

        Assert.True(limiter.TryAcquire(out int refilledRetryAfterMilliseconds));
        Assert.Equal(0, refilledRetryAfterMilliseconds);
    }
}
