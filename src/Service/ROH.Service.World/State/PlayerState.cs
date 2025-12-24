using ROH.Contracts.WebSocket.Player;

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ROH.Service.World.State;

public sealed class PlayerState
{
    public string PlayerId { get; }
    public Vector3 Position { get; private set; }
    public Quaternion Rotation { get; private set; }
    public DateTime LastInputAt { get; private set; }

    public PlayerState(string playerId, Vector3 position, Quaternion rotation)
    {
        PlayerId = playerId;
        Position = position;
        Rotation = rotation;
        LastInputAt = DateTime.UtcNow;
    }

    public void ApplyInput(PlayerInput input)
    {
        Position = new Vector3(input.X, input.Y, input.Z);
        Rotation = new Quaternion(input.RotX, input.RotY, input.RotZ, input.RotW);
        LastInputAt = DateTime.UtcNow;
    }
}

