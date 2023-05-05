using ROH.Domain.Characters;

namespace ROH.Domain.Kingdoms
{
    public record Champion(long Id, long IdCharacter, long IdKingdom)
    {
        public virtual Character? Character { get; set; }
        public virtual Kingdom? Kingdom { get; set; }
    }
}