//-----------------------------------------------------------------------
// <copyright file="PlayerState.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace ROH.StandardModels.Character.Position
{
    public class PlayerState
    {
        public int AnimationState { get; set; }

        public string PlayerId { get; set; } = string.Empty;

        public float PositionX { get; set; }

        public float PositionY { get; set; }

        public float PositionZ { get; set; }

        public float RotationW { get; set; }

        public float RotationX { get; set; }

        public float RotationY { get; set; }

        public float RotationZ { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}