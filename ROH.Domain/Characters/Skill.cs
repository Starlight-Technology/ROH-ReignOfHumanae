using ROH.Domain.Effect;

namespace ROH.Domain.Characters
{
    public record Skill(long Id, long IdCharacter, long? Damage, long? Defense, long ManaCost, string? Animation, string Name, EffectType Type);
}