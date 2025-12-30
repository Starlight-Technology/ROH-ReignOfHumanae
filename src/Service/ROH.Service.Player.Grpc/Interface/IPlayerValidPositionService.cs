using ROH.StandardModels.Character.Position;

namespace ROH.Service.Player.Grpc.Interface;

public interface IPlayerValidPositionService
{
    PlayerPositionValidationResult Validate(PlayerPositionInput input);
}
