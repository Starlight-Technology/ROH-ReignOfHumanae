using MessagePack;

namespace ROH.Contracts.WebSocket
{
    [MessagePackObject]
    public sealed class WorldSnapshot
    {
        [Key(0)] public long ServerTick;

        //[Key(1)] public PlayerSnapshot[] Players;
    }

}
