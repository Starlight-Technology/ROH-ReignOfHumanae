using ROH.Domain.Characters;

namespace ROH.Domain.Guilds
{
    public record Guild(long Id, string Name, string Description, ICollection<Character> Characters, ICollection<MembersPosition> Positions)
    {
        public Guild(string name, string description, ICollection<Character> characters, ICollection<MembersPosition> positions) : this(default, name ?? throw new ArgumentNullException(nameof(name)), description ?? throw new ArgumentNullException(nameof(description)), characters ?? throw new ArgumentNullException(nameof(characters)), positions ?? throw new ArgumentNullException(nameof(positions)))
        {
        }
    }
}
