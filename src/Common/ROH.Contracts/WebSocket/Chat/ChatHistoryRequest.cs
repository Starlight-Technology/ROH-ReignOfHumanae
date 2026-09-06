using MessagePack;

namespace ROH.Contracts.WebSocket.Chat;

[MessagePackObject]
public class ChatHistoryRequest
{
    [Key(0)] public ChatChannel Channel;
    [Key(1)] public int Limit;
}
