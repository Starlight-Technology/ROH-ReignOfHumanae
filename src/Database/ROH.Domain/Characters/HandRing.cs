using ROH.Domain.items;

namespace ROH.Domain.Characters;

public record HandRing(long Id, long IdEquippedItems, long IdItem)
{
    public virtual EquippedItems? EquippedItems { get; set; }
    public virtual Item? Item { get; set; }
}