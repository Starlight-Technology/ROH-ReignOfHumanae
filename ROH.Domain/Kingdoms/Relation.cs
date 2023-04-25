namespace ROH.Domain.Kingdoms
{
    public record Relation(long Id, long IdKingdom, long IdKingdom2, Situation Situation, Kingdom Kingdom, Kingdom Kingdom2);
}