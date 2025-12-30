//-----------------------------------------------------------------------
// <copyright file="CharacterSkill.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ROH.StandardModels.Character
{
    public class CharacterSkill
    {
        public CharacterSkill() => Skill = new Skill();

        public long Id { get; set; }

        public long IdCharacter { get; set; }

        public long IdSkill { get; set; }

        public virtual Skill Skill { get; set; }
    }
}