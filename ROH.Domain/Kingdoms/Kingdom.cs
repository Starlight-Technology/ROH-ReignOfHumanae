using ROH.Domain.Characters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Domain.Kingdoms
{
    public record Kingdom(int Id, int IdRuler, Reign Reign, Character? Ruler, ICollection<Character>? Citzens, ICollection<Character>? Champions, ICollection<Relation>? KingdomRelations);
}
