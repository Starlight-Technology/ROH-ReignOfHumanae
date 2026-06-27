using MessagePack;

namespace ROH.Contracts.WebSocket.Chat;

[MessagePackObject]
public class ChatHistoryResponse
{
    [Key(0)] public ChatChannel Channel;
    [Key(1)] public ChatMessage[] Messages = [];
}
