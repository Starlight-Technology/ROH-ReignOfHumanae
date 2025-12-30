//-----------------------------------------------------------------------
// <copyright file="CharacterInventory.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ROH.StandardModels.Character
{
    public class CharacterInventory
    {
        public long Id { get; set; }

        public long IdCharacter { get; set; }

        public long IdItem { get; set; }
    }
}