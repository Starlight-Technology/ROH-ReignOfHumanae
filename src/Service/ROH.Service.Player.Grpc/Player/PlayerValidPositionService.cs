using ROH.Service.Player.Grpc.Interface;
using ROH.StandardModels.Character.Position;

using System.Numerics;

namespace ROH.Service.Player.Grpc.Player;

public class PlayerValidPositionService : IPlayerValidPositionService
{
    private const float MAX_SPEED = 6f; // m/s
    private const float MAX_TELEPORT_DISTANCE = 15f;
    private const float MAX_TIME_DESYNC = 0.5f;

    public PlayerPositionValidationResult Validate(PlayerPositionInput input)
    {
        float deltaTime = (float)(input.ServerTimestamp - input.LastServerTimestamp).TotalSeconds;

        if (deltaTime <= 0 || deltaTime > MAX_TIME_DESYNC)
            return PlayerPositionValidationResult.InvalidTimestamp;

        float distance = Vector3.Distance(
            input.LastServerPosition,
            input.ClientReportedPosition);

        float speed = distance / deltaTime;

        if (distance > MAX_TELEPORT_DISTANCE)
            return PlayerPositionValidationResult.InvalidTeleport;

        if (speed > MAX_SPEED)
            return PlayerPositionValidationResult.InvalidSpeed;

        return PlayerPositionValidationResult.Valid;
    }
}