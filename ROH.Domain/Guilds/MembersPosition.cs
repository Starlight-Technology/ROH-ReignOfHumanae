using ROH.Domain.Characters;

namespace ROH.Domain.Guilds
{
    public record MembersPosition(long Id, long IdCharacter, long IdGuild, Position Position)
    {
        public MembersPosition(Position position) : this(default, default, default, position)
        {
        }

        public virtual Character? Character { get; set; }
        public virtual Guild? Guild { get; set; }
    }
}