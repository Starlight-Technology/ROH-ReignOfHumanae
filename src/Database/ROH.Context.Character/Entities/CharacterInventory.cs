//-----------------------------------------------------------------------
// <copyright file="CharacterInventory.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ROH.Context.Character.Entities;

public record CharacterInventory(long Id, long IdItem, long IdCharacter)
{
    public virtual Character? Character { get; set; }

}