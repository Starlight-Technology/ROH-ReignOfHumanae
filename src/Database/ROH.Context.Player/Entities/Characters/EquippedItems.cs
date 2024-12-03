//-----------------------------------------------------------------------
// <copyright file="EquippedItems.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ROH.Context.Player.Entities.Characters;

public record EquippedItems(
    long IdCharacter,
    long? IdArmor,
    long? IdHead,
    long? IdBoots,
    long? IdGloves,
    long? IdLegs,
    long? IdLeftBracelet,
    long? IdNecklace,
    long? IdRightBracelet)
{
    public virtual Character? Character { get; set; }

    public virtual ICollection<HandRing>? LeftHandRings { get; set; }

    public virtual ICollection<HandRing>? RightHandRings { get; set; }
}