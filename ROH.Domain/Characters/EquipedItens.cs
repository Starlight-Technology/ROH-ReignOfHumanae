using ROH.Domain.Itens;

namespace ROH.Domain.Characters
{
    public record EquipedItens(long IdCharacter, Item? Armor, Item? Head, Item? Boots, Item? Gloves, Item? Legs, Item? LeftBracelet, Item? Necklace, Item? RightBracelet, Character? Character, ICollection<Item>? LeftHandRings, ICollection<Item>? RightHandRings);
}