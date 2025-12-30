//-----------------------------------------------------------------------
// <copyright file="IPlayerValidPositionService.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.StandardModels.Character.Position;

namespace ROH.Service.Player.Grpc.Interface;

public interface IPlayerValidPositionService
{
    PlayerPositionValidationResult Validate(PlayerPositionInput input);
}