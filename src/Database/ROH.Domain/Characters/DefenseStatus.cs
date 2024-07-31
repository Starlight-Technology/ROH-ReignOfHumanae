﻿namespace ROH.Domain.Characters;

public record DefenseStatus(long IdCharacter,
                            long ArcaneDefenseLevel,
                            long DarknessDefenseLevel,
                            long EarthDefenseLevel,
                            long FireDefenseLevel,
                            long LightDefenseLevel,
                            long LightningDefenseLevel,
                            long MagicDefenseLevel,
                            long PhysicDefenseLevel,
                            long WaterDefenseLevel,
                            long WindDefenseLevel)
{
    public virtual Character? Character { get; set; }
}