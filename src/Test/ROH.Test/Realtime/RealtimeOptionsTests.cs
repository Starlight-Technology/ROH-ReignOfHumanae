using ROH.Gateway.Realtime;

namespace ROH.Test.Realtime;

public class RealtimeOptionsTests
{
    [Fact]
    public void ApplySafeDefaults_ShouldReplaceInvalidValues()
    {
        RealtimeOptions options = new()
        {
            ChatCleanupIntervalHours = 0,
            ChatHistoryLimit = 0,
            ChatMaxMessageLength = 0,
            ChatRateLimitBurst = 0,
            ChatRateLimitMessagesPerSecond = 0,
            ChatRetentionHours = 0,
            PlayerPositionSaveIntervalSeconds = 0
        };

        options.ApplySafeDefaults();

        Assert.Equal(6, options.ChatCleanupIntervalHours);
        Assert.Equal(50, options.ChatHistoryLimit);
        Assert.Equal(300, options.ChatMaxMessageLength);
        Assert.Equal(3, options.ChatRateLimitBurst);
        Assert.Equal(1, options.ChatRateLimitMessagesPerSecond);
        Assert.Equal(72, options.ChatRetentionHours);
        Assert.Equal(5, options.PlayerPositionSaveIntervalSeconds);
    }
}
