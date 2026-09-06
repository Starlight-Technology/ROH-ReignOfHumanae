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
    private const float MAX_ABSOLUTE_COORDINATE = 1_000_000f;
    private const float MAX_SPEED = 6f; // m/s
    private const float MAX_TELEPORT_DISTANCE = 15f;
    private const float MAX_TIME_DESYNC = 0.5f;

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

        if (deltaTime is <= 0 or > MAX_TIME_DESYNC)
            return PlayerPositionValidationResult.InvalidTimestamp;

        float distance = Vector3.Distance(input.LastServerPosition, input.ClientReportedPosition);

        float speed = distance / deltaTime;

        return distance > MAX_TELEPORT_DISTANCE
            ? PlayerPositionValidationResult.InvalidTeleport
            : speed > MAX_SPEED ? PlayerPositionValidationResult.InvalidSpeed : PlayerPositionValidationResult.Valid;
    }

    private static bool IsFinite(float value) => !float.IsNaN(value) && !float.IsInfinity(value);

    private static bool IsFiniteAndBounded(float value) =>
        IsFinite(value) && Math.Abs(value) <= MAX_ABSOLUTE_COORDINATE;
}
