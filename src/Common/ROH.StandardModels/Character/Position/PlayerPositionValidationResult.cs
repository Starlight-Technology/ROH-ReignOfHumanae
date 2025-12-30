//-----------------------------------------------------------------------
// <copyright file="PlayerPositionValidationResult.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ROH.StandardModels.Character.Position
{
    public enum PlayerPositionValidationResult
    {
        Valid,
        InvalidSpeed,
        InvalidTeleport,
        InvalidTimestamp
    }
}