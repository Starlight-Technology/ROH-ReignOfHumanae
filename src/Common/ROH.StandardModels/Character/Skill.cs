//-----------------------------------------------------------------------
// <copyright file="Skill.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ROH.StandardModels.Character
{
    public class Skill
    {
        public string? Animation { get; set; }

        public long? Damage { get; set; }

        public long? Defense { get; set; }

        public long Id { get; set; }

        public long ManaCost { get; set; }

        public string? Name { get; set; }

        public EffectType Type { get; set; }
    }
}