using MessagePack;

namespace ROH.Contracts.WebSocket;

[MessagePackObject]
public class RealtimeEnvelope
{
    [Key(0)] public string Type;
    [Key(1)] public byte[] Payload;
}