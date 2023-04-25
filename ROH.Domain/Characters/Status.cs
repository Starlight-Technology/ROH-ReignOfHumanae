namespace ROH.Domain.Characters
{
    public record Status(long IdCharacter, long Level, long MagicLevel, long FullCarryWeight, long CurrentCarryWeight, long FullHealth, long CurrentHealth, long FullMana, long CurrentMana, long FullStamina, long CurrentStamina, Character? Character);
}