using ROH.Domain.Itens;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Domain.Characters
{
    public record CharacterInventory(long Id, long IdItem, long IdCharacter)
    {
        public virtual Character Character { get; set; }
        public virtual Item Item { get; set; }
    }
}
