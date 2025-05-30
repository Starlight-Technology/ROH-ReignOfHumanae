using ROH.Context.Player.Mongo.Entities;

namespace ROH.Context.Player.Mongo.Interface;

public interface IPositionRepository
{
    Task<List<PlayerPosition>> GetAllPlayersAsync(CancellationToken cancellationToken = default);
    Task SavePlayerPositionAsync(PlayerPosition data, CancellationToken cancellationToken = default);
}
