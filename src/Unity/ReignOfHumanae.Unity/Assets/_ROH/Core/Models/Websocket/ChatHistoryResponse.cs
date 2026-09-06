using MessagePack;

namespace Assets.Scripts.Models.Websocket
{
    [MessagePackObject]
    public class ChatHistoryResponse
    {
        [Key(0)] public ChatChannel Channel;
        [Key(1)] public ChatMessageModel[] Messages;
    }
}
