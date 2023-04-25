using ROH.Domain.Characters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Domain.Guilds
{
    public class Guild
    {
        public Guild(string name, string description, ICollection<Character> characters, ICollection<MembersPosition> positions)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Characters = characters ?? throw new ArgumentNullException(nameof(characters));
            Positions = positions ?? throw new ArgumentNullException(nameof(positions));
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Character> Characters { get; set; }
        public virtual ICollection<MembersPosition> Positions { get; set; }

    }
}
