//-----------------------------------------------------------------------
// <copyright file="Position.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ROH.Context.Player.Entities.Characters;

public record Position(long Id = 0, long IdPlayer = 0, float X = 0, float Y = 0, float Z = 0);