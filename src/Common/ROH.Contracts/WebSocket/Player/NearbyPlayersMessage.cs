using MessagePack;

namespace ROH.Contracts.WebSocket.Player
{
    [MessagePackObject]
    public class NearbyPlayersMessage
    {
        [Key(0)] public List<Player.NearbyPlayerMessage> Players = new List<Player.NearbyPlayerMessage>();
    }
}