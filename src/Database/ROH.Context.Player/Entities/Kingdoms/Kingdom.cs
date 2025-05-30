//-----------------------------------------------------------------------
// <copyright file="Kingdom.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.Context.Player.Entities.Characters;

namespace ROH.Context.Player.Entities.Kingdoms;

public record Kingdom(long Id, long IdRuler, Reign Reign)
{
    public virtual ICollection<KingdomRelation>? OutgoingRelations { get; set; }
    public virtual ICollection<KingdomRelation>? IncomingRelations { get; set; }

    public virtual ICollection<Champion>? Champions { get; set; }
    public virtual ICollection<Character>? Citizens { get; set; }
    public virtual Character? Ruler { get; set; }
}
