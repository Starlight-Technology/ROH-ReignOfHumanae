namespace ROH.Domain.Kingdoms;

public record KingdomRelation(long Id, long IdKingdom, long IdKingdom2, Situation Situation)
{
    public virtual Kingdom? Kingdom { get; set; }
    public virtual Kingdom? Kingdom2 { get; set; }
}