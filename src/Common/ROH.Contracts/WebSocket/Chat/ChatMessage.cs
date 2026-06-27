using MessagePack;

namespace ROH.Contracts.WebSocket.Chat;

[MessagePackObject]
public class ChatMessage
{
    [Key(0)] public string Id = string.Empty;
    [Key(1)] public ChatChannel Channel;
    [Key(2)] public string SenderCharacterId = string.Empty;
    [Key(3)] public string SenderDisplayName = string.Empty;
    [Key(4)] public string? TargetCharacterId;
    [Key(5)] public string[] Tags = [];
    [Key(6)] public string Message = string.Empty;
    [Key(7)] public DateTime CreatedAtUtc;
}
