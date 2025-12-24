using MessagePack;

using ROH.Contracts.WebSocket.Player;

namespace ROH.Contracts.WebSocket
{
    [MessagePackObject]
    public sealed class WorldSnapshot
    {
        [Key(0)] public long ServerTick;

        [Key(1)] public PlayerSnapshot[] Players;
    }

}
