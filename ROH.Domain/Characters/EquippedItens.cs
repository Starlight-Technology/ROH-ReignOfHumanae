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
                               long? IdRightBracelet)
    {
        public virtual Item? Armor { get; set; }
        public virtual Item? Head { get; set; }
        public virtual Item? Boots { get; set; }
        public virtual Item? Gloves { get; set; }
        public virtual Item? Legs { get; set; }
        public virtual Item? LeftBracelet { get; set; }
        public virtual Item? Necklace { get; set; }
        public virtual Item? RightBracelet { get; set; }
        public virtual Character? Character { get; set; }
        public virtual ICollection<HandRing>? LeftHandRings { get; set; }
        public virtual ICollection<HandRing>? RightHandRings { get; set; }
    }
}