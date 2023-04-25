namespace ROH.Domain.Characters
{
    public class DefenseStatus
    {
        public long IdCharacter { get; set; }
        public long ArcaneDefenseLevel { get; set; }
        public long DarkDefenseLevel { get; set; }
        public long EarthDefenseLevel { get; set; }
        public long FireDefenseLevel { get; set; }
        public long LightDefenseLevel { get; set; }
        public long MagicDefenseLevel { get; set; }
        public long PhysicsDefenseLevel { get; set; }
        public long WaterDefenseLevel { get; set; }
        public long WindDefenseLevel { get; set; }
        public virtual Character? Character { get; set; }
    }
}