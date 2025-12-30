using ROH.Service.Player.Grpc.Player;
using ROH.StandardModels.Character.Position;

using System;
using System.Numerics;

using Xunit;

namespace ROH.Test.Player.Validation;

public class PlayerValidPositionServiceTest
{
    private readonly PlayerValidPositionService _service;
    private readonly Guid _playerId = Guid.NewGuid();

    public PlayerValidPositionServiceTest()
    {
        _service = new PlayerValidPositionService();
    }

    [Fact]
    public void Validate_ShouldReturnValid_WhenMovementIsWithinLimits()
    {
        DateTime lastServerTime = DateTime.UtcNow;
        DateTime serverTime = lastServerTime.AddSeconds(0.2);

        PlayerPositionInput input = new(
            _playerId,
            Vector3.Zero,
            new Vector3(1, 0, 0),
            lastServerTime,
            serverTime
        );

        PlayerPositionValidationResult result = _service.Validate(input);

        Assert.Equal(PlayerPositionValidationResult.Valid, result);
    }

    [Fact]
    public void Validate_ShouldReturnInvalidSpeed_WhenSpeedIsTooHigh()
    {
        DateTime lastServerTime = DateTime.UtcNow;
        DateTime serverTime = lastServerTime.AddSeconds(0.1);

        PlayerPositionInput input = new(
            _playerId,
            Vector3.Zero,
            new Vector3(10, 0, 0),
            lastServerTime,
            serverTime
        );

        PlayerPositionValidationResult result = _service.Validate(input);

        Assert.Equal(PlayerPositionValidationResult.InvalidSpeed, result);
    }

    [Fact]
    public void Validate_ShouldReturnInvalidTeleport_WhenDistanceIsTooLarge()
    {
        DateTime lastServerTime = DateTime.UtcNow;
        DateTime serverTime = lastServerTime.AddSeconds(0.3);

        PlayerPositionInput input = new(
            _playerId,
            Vector3.Zero,
            new Vector3(50, 0, 0),
            lastServerTime,
            serverTime
        );

        PlayerPositionValidationResult result = _service.Validate(input);

        Assert.Equal(PlayerPositionValidationResult.InvalidTeleport, result);
    }

    [Fact]
    public void Validate_ShouldReturnInvalidTimestamp_WhenClientTimeIsAhead()
    {
        DateTime lastServerTime = DateTime.UtcNow;
        DateTime serverTime = lastServerTime.AddSeconds(-0.5);

        PlayerPositionInput input = new(
            _playerId,
            Vector3.Zero,
            new Vector3(1, 0, 0),
            lastServerTime,
            serverTime
        );

        PlayerPositionValidationResult result = _service.Validate(input);

        Assert.Equal(PlayerPositionValidationResult.InvalidTimestamp, result);
    }

    [Fact]
    public void Validate_ShouldReturnInvalidTimestamp_WhenDeltaTimeIsTooLarge()
    {
        DateTime lastServerTime = DateTime.UtcNow;
        DateTime serverTime = lastServerTime.AddSeconds(10);

        PlayerPositionInput input = new(
            _playerId,
            Vector3.Zero,
            new Vector3(1, 0, 0),
            lastServerTime,
            serverTime
        );

        PlayerPositionValidationResult result = _service.Validate(input);

        Assert.Equal(PlayerPositionValidationResult.InvalidTimestamp, result);
    }
}
