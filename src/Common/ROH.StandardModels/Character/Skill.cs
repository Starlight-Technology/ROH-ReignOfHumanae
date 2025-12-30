namespace ROH.StandardModels.Character
{
    public class Skill
    {
        public long Id { get; set; }
        public long? Damage { get; set; }
        public long? Defense { get; set; }
        public long ManaCost { get; set; }
        public string? Animation { get; set; }
        public string? Name { get; set; }
        public EffectType Type { get; set; }
    }
}