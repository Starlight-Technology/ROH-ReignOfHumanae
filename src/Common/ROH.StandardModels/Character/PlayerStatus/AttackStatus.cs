//-----------------------------------------------------------------------
// <copyright file="AttackStatus.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ROH.StandardModels.Character.PlayerStatus
{
    public class AttackStatus
    {
        public long IdCharacter { get; set; }

        public long LongRangedWeaponLevel { get; set; }

        public long MagicWeaponLevel { get; set; }

        public long OneHandedWeaponLevel { get; set; }

        public long TwoHandedWeaponLevel { get; set; }
    }
}