//-----------------------------------------------------------------------
// <copyright file="Status.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ROH.StandardModels.Character.PlayerStatus
{
    public class Status
    {
        public long CurrentCarryWeight { get; set; }

        public long CurrentHealth { get; set; }

        public long CurrentMana { get; set; }

        public long CurrentStamina { get; set; }

        public long FullCarryWeight { get; set; }

        public long FullHealth { get; set; }

        public long FullMana { get; set; }

        public long FullStamina { get; set; }

        public long IdCharacter { get; set; }

        public long Level { get; set; }

        public long MagicLevel { get; set; }
    }
}