//-----------------------------------------------------------------------
// <copyright file="Kingdom.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.Domain.Characters;

namespace ROH.Domain.Kingdoms;

public record Kingdom(long Id, long IdRuler, Reign Reign)
{
    public virtual ICollection<Champion>? Champions { get; set; }

    public virtual ICollection<Character>? Citizens { get; set; }

    public virtual ICollection<KingdomRelation>? KingdomRelations { get; set; }

    public virtual Character? Ruler { get; set; }
}