//-----------------------------------------------------------------------
// <copyright file="CharacterInventory.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

//-----------------------------------------------------------------------
// <copyright file="CharacterInventory.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ROH.Context.Player.Entities.Characters;

public record CharacterInventory(long Id, long IdItem, long IdCharacter)
{
    public virtual Character? Character { get; set; }

}