using ROH.Domain.Characters;

namespace ROH.Domain.Guilds
{
    public record MembersPosition(long Id, long IdCharacter, long IdGuild, Position Position, Character Character, Guild Guild)
    {
        public MembersPosition(Position position, Character character, Guild guild) : this(default, default, default, position, character ?? throw new ArgumentNullException(nameof(character)), guild ?? throw new ArgumentNullException(nameof(guild)))
        {
        }
    }
}