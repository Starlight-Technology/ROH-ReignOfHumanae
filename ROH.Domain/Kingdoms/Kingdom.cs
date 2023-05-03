using ROH.Domain.Characters;

namespace ROH.Domain.Kingdoms
{
    public record Kingdom(int Id, int IdRuler, Reign Reign, Character? Ruler, ICollection<Character>? Citzens, ICollection<Champion>? Champions, ICollection<KingdomRelation>? KingdomRelations);
}
