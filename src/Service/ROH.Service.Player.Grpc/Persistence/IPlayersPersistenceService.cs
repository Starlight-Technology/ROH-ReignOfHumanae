using ROH.Contracts.GRPC.Player.NearbyPlayer;
using ROH.Contracts.GRPC.Player.PlayerPosition;
using ROH.StandardModels.Character.Position;

namespace ROH.Service.Player.Grpc.Persistence;

public interface IPlayersPersistenceService
{
    Task<ICollection<PlayerInfo>> GetNearbyPlayerAsync(string playerId, float radius, int maxPlayers, CancellationToken cancellationToken);
    Task SavePlayerPosition(PlayerRequest player, CancellationToken token);
    Task<PlayerState?> GetPlayerState(string guid);
}
