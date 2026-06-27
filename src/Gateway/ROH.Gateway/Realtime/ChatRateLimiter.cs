namespace ROH.Gateway.Realtime;

public sealed class ChatRateLimiter
{
    readonly int _capacity;
    readonly double _messagesPerSecond;
    readonly object _sync = new();
    readonly Func<DateTime> _utcNow;
    DateTime _lastRefillUtc;
    double _tokens;

    public ChatRateLimiter(double messagesPerSecond, int burst, Func<DateTime>? utcNow = null)
    {
        _utcNow = utcNow ?? (() => DateTime.UtcNow);
        _messagesPerSecond = messagesPerSecond > 0 ? messagesPerSecond : 1;
        _capacity = burst > 0 ? burst : 1;
        _lastRefillUtc = _utcNow();
        _tokens = _capacity;
    }

    public bool TryAcquire(out int retryAfterMilliseconds)
    {
        lock (_sync)
        {
            DateTime nowUtc = _utcNow();
            double elapsedSeconds = (nowUtc - _lastRefillUtc).TotalSeconds;
            _tokens = Math.Min(_capacity, _tokens + (elapsedSeconds * _messagesPerSecond));
            _lastRefillUtc = nowUtc;

            if (_tokens >= 1)
            {
                _tokens -= 1;
                retryAfterMilliseconds = 0;
                return true;
            }

            retryAfterMilliseconds = (int)Math.Ceiling(((1 - _tokens) / _messagesPerSecond) * 1000);
            return false;
        }
    }
}
