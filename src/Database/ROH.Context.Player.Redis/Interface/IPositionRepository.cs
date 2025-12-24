using ROH.Context.Player.Redis.Entities;

namespace ROH.Context.Player.Redis.Interface;

public interface IPositionRepository
{
    Task SavePlayerPosition(PlayerPositionRedis position, CancellationToken cancellationToken);
}
