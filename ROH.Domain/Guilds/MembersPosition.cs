using ROH.Domain.Characters;

namespace ROH.Domain.Guilds
{
    public class MembersPosition
    {
        public MembersPosition(Position position, Character character, Guild guild)
        {
            Position = position;
            Character = character ?? throw new ArgumentNullException(nameof(character));
            Guild = guild ?? throw new ArgumentNullException(nameof(guild));
        }

        public int Id { get; set; }
        public int IdCharacter { get; set; }
        public int IdGuild { get; set; }
        public Position Position{ get; set; }

        public virtual Character Character { get; set; }
        public virtual Guild Guild { get; set; }
    }
}