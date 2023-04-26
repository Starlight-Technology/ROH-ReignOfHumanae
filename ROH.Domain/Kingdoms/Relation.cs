namespace ROH.Domain.Kingdoms
{
    public record KingdomRelation(long Id, long IdKingdom, long IdKingdom2, Situation Situation, Kingdom Kingdom, Kingdom Kingdom2);
}