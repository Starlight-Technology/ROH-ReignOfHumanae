//-----------------------------------------------------------------------
// <copyright file="MembersPosition.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.Context.Player.Entities.Characters;

namespace ROH.Context.Player.Entities.Guilds;

public record MembersPosition(long Id, long IdCharacter, long IdGuild, Position Position)
{
    public MembersPosition(Position position) : this(default, default, default, position)
    {
    }

    public virtual Character? Character { get; set; }

    public virtual Guild? Guild { get; set; }
}