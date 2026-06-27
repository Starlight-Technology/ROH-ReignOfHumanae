using MessagePack;

namespace Assets.Scripts.Models.Websocket
{
    [MessagePackObject]
    public class DisconnectNoticeMessage
    {
        [Key(0)] public string Reason;
        [Key(1)] public string Message;
    }
}
