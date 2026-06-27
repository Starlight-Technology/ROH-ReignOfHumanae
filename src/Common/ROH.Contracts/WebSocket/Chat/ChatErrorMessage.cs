using MessagePack;

namespace ROH.Contracts.WebSocket.Chat;

[MessagePackObject]
public class ChatErrorMessage
{
    [Key(0)] public string Code = string.Empty;
    [Key(1)] public string Message = string.Empty;
    [Key(2)] public int RetryAfterMilliseconds;
}
