using ROH.Domain.Characters;

namespace ROH.Domain.Kingdoms
{
    public record Champion(long Id, long IdCharacter, long IdKingdom, Character Character, Kingdom Kingdom)
    {
    }
}