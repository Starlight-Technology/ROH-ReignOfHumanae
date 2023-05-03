using ROH.Domain.Itens;

namespace ROH.Domain.Characters
{
    public record EquippedItens(long IdCharacter,
                               long? IdArmor,
                               long? IdHead,
                               long? IdBoots,
                               long? IdGloves,
                               long? IdLegs,
                               long? IdLeftBracelet,
                               long? IdNecklace,
                               long? IdRightBracelet,
                               Item? Armor,
                               Item? Head,
                               Item? Boots,
                               Item? Gloves,
                               Item? Legs,
                               Item? LeftBracelet,
                               Item? Necklace,
                               Item? RightBracelet,
                               Character? Character,
                               ICollection<HandRing>? LeftHandRings,
                               ICollection<HandRing>? RightHandRings);
}