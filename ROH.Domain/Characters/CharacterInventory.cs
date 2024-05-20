using ROH.Domain.items;

namespace ROH.Domain.Characters;

public record CharacterInventory(long Id, long IdItem, long IdCharacter)
{
    public virtual Character? Character { get; set; }
    public virtual Item? Item { get; set; }
}