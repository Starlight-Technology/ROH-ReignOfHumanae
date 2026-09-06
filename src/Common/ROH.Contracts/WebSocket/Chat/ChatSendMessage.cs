using MessagePack;

namespace ROH.Contracts.WebSocket.Chat;

[MessagePackObject]
public class ChatSendMessage
{
    [Key(0)] public ChatChannel Channel;
    [Key(1)] public string Message = string.Empty;
}
