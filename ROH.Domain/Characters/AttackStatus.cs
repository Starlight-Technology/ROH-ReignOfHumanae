namespace ROH.Domain.Characters
{
    public class AttackStatus
    {
        public long IdCharacter { get; set; }
        public long LongRangedWeaponLevel { get; set; }
        public long MagicWeaponLevel { get; set; }
        public long OneHandedWeaponLevel { get; set; }
        public long TwoHandedWeaponLevel { get; set; }
        public virtual Character? Character { get; set; }
    }
}