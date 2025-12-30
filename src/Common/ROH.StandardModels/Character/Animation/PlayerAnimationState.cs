//-----------------------------------------------------------------------
// <copyright file="PlayerAnimationState.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ROH.StandardModels.Character.Animation
{
    public enum PlayerAnimationState
    {
        // Ground
        GroundIdle,

        GroundWalk,
        GroundRun,
        GroundJump,

        // Swimming
        SwimmingIdle,

        SwimmingMove,

        // Flying
        FlyingIdle,

        FlyingMove
    }
}