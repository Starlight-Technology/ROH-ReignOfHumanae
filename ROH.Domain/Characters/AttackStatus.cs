namespace ROH.Domain.Characters
{
    public record AttackStatus(long IdCharacter, long LongRangedWeaponLevel, long MagicWeaponLevel, long OneHandedWeaponLevel, long TwoHandedWeaponLevel)
    {
        public virtual Character Character { get; set; }
    }
}