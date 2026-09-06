using MessagePack;

namespace ROH.Contracts.WebSocket.Session;

[MessagePackObject]
public class DisconnectNoticeMessage
{
    [Key(0)] public string Reason = string.Empty;
    [Key(1)] public string Message = string.Empty;
}
