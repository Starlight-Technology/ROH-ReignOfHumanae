//-----------------------------------------------------------------------
// <copyright file="CharacterSkill.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------



//-----------------------------------------------------------------------
// <copyright file="CharacterSkill.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ROH.Context.Player.Entities.Characters;

public record CharacterSkill(long Id, long IdCharacter, long IdSkill)
{
    public virtual Character? Character { get; set; }

    public virtual Skill? Skill { get; set; }
}