using ROH.Domain.Itens;

namespace ROH.Domain.Characters
{
    public record HandRing(long Id, long IdEquippedItens, long IdItem)
    {
        public virtual EquippedItens? EquippedItens { get; set; }
        public virtual Item? Item { get; set; }
    }
}