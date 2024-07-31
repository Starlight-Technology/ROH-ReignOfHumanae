using ROH.Domain.Characters;

namespace ROH.Domain.Kingdoms;

public record Kingdom(long Id, long IdRuler, Reign Reign)
{
    public virtual Character? Ruler { get; set; }
    public virtual ICollection<Character>? Citizens { get; set; }
    public virtual ICollection<Champion>? Champions { get; set; }
    public virtual ICollection<KingdomRelation>? KingdomRelations { get; set; }
}