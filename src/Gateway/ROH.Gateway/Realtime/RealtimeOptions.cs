namespace ROH.Gateway.Realtime;

public class RealtimeOptions
{
    public int ChatCleanupIntervalHours { get; set; } = 6;

    public int ChatHistoryLimit { get; set; } = 50;

    public int ChatMaxMessageLength { get; set; } = 300;

    public int ChatRateLimitBurst { get; set; } = 3;

    public double ChatRateLimitMessagesPerSecond { get; set; } = 1;

    public int ChatRetentionHours { get; set; } = 72;

    public int PlayerPositionSaveIntervalSeconds { get; set; } = 5;

    public void ApplySafeDefaults()
    {
        if (ChatCleanupIntervalHours <= 0)
            ChatCleanupIntervalHours = 6;
        if (ChatHistoryLimit <= 0)
            ChatHistoryLimit = 50;
        if (ChatMaxMessageLength <= 0)
            ChatMaxMessageLength = 300;
        if (ChatRateLimitBurst <= 0)
            ChatRateLimitBurst = 3;
        if (ChatRateLimitMessagesPerSecond <= 0)
            ChatRateLimitMessagesPerSecond = 1;
        if (ChatRetentionHours < 1)
            ChatRetentionHours = 72;
        if (PlayerPositionSaveIntervalSeconds <= 0)
            PlayerPositionSaveIntervalSeconds = 5;
    }
}
