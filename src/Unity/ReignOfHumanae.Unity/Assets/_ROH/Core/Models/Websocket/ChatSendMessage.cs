using MessagePack;

namespace Assets.Scripts.Models.Websocket
{
    [MessagePackObject]
    public class ChatSendMessage
    {
        [Key(0)] public ChatChannel Channel;
        [Key(1)] public string Message;
    }
}
