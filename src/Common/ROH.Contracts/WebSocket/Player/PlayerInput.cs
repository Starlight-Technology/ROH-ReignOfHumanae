using MessagePack;

namespace ROH.Contracts.WebSocket.Player
{
    [MessagePackObject]
    public sealed class PlayerInput
    {
        [Key(0)] public string PlayerId;
        [Key(1)] public float X;
        [Key(2)] public float Y;
        [Key(3)] public float Z;

        [Key(4)] public float RotX;
        [Key(5)] public float RotY;
        [Key(6)] public float RotZ;
        [Key(7)] public float RotW;

        // Timestamp do client (latência, interpolação, debug)
        [Key(8)] public long ClientTick;
    }

}
