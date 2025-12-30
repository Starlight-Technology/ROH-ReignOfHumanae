//-----------------------------------------------------------------------
// <copyright file="PlayerPositionRedis.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ROH.Context.Player.Redis.Entities;

public class PlayerPositionRedis
{
    public int PlayerAnimationState { get; set; }

    public string PlayerId { get; set; } = default!;

    public float PositionX { get; set; } // longitude

    public float PositionY { get; set; }

    public float PositionZ { get; set; } // latitude

    public float RotationW { get; set; }

    public float RotationX { get; set; }

    public float RotationY { get; set; }

    public float RotationZ { get; set; }

    public long Timestamp { get; set; } // Unix milliseconds
}