using ROH.Domain.Itens;

namespace ROH.Domain.Characters
{
    public record HandRing(long Id, long IdEquippedItens, long IdItem, EquippedItens EquippedItens, Item Item)
    {
    }
}