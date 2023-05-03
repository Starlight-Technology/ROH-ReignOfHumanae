using ROH.Domain.Characters;

namespace ROH.Domain.Guilds
{
    public record Guild(long Id,
                        string Name,
                        string Description)
    {
        public Guild(string name,
                     string description) : this(
                         default,
                         name ?? throw new ArgumentNullException(nameof(name)),
                         description ?? throw new ArgumentNullException(nameof(description)))
        {
        }

        public virtual ICollection<Character>? Characters { get; set; }
        public virtual ICollection<MembersPosition>? MembersPositions { get; set; }
    }
}
