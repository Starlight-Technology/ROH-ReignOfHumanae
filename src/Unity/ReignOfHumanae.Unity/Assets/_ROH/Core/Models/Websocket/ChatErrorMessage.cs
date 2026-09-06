using MessagePack;

namespace Assets.Scripts.Models.Websocket
{
    [MessagePackObject]
    public class ChatErrorMessage
    {
        [Key(0)] public string Code;
        [Key(1)] public string Message;
        [Key(2)] public int RetryAfterMilliseconds;
    }
}
