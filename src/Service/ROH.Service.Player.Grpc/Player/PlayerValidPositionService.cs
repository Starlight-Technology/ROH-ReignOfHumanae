//-----------------------------------------------------------------------
// <copyright file="PlayerValidPositionService.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.Service.Player.Grpc.Interface;
using ROH.StandardModels.Character.Position;

using System.Numerics;

namespace ROH.Service.Player.Grpc.Player;

public class PlayerValidPositionService : IPlayerValidPositionService
{
    const float MAX_ABSOLUTE_COORDINATE = 1_000_000f;
    const float MAX_SPEED = 6f; // m/s
    const float MAX_TELEPORT_DISTANCE = 15f;
    const float MAX_TIME_DESYNC = 0.5f;

    public PlayerPositionValidationResult Validate(PlayerPositionInput input)
    {
        if (!IsFiniteAndBounded(input.ClientReportedPosition.X)
            || !IsFiniteAndBounded(input.ClientReportedPosition.Y)
            || !IsFiniteAndBounded(input.ClientReportedPosition.Z)
            || !IsFinite(input.ClientReportedRotation.X)
            || !IsFinite(input.ClientReportedRotation.Y)
            || !IsFinite(input.ClientReportedRotation.Z)
            || !IsFinite(input.ClientReportedRotation.W))
        {
            return PlayerPositionValidationResult.InvalidCoordinates;
        }

        float deltaTime = (float)(input.ServerTimestamp - input.LastServerTimestamp).TotalSeconds;

        if ((deltaTime <= 0) || (deltaTime > MAX_TIME_DESYNC))
            return PlayerPositionValidationResult.InvalidTimestamp;

        float distance = Vector3.Distance(input.LastServerPosition, input.ClientReportedPosition);

        float speed = distance / deltaTime;

        if (distance > MAX_TELEPORT_DISTANCE)
            return PlayerPositionValidationResult.InvalidTeleport;

        if (speed > MAX_SPEED)
            return PlayerPositionValidationResult.InvalidSpeed;

        return PlayerPositionValidationResult.Valid;
    }

    static bool IsFinite(float value) => !float.IsNaN(value) && !float.IsInfinity(value);

    static bool IsFiniteAndBounded(float value) =>
        IsFinite(value) && Math.Abs(value) <= MAX_ABSOLUTE_COORDINATE;
}
