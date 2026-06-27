using MessagePack;

namespace ROH.Contracts.WebSocket.Player;

[MessagePackObject]
public class InitialPlayerStateMessage
{
    [Key(0)] public string CharacterId = string.Empty;
    [Key(1)] public string WorldId = string.Empty;
    [Key(2)] public float X;
    [Key(3)] public float Y;
    [Key(4)] public float Z;
    [Key(5)] public float RotX;
    [Key(6)] public float RotY;
    [Key(7)] public float RotZ;
    [Key(8)] public float RotW;
    [Key(9)] public bool RestoredFromPersistence;
}
