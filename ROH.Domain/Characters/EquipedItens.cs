using ROH.Domain.Itens;

namespace ROH.Domain.Characters
{
    public class EquipedItens
    {
        public long IdCharacter { get; set; }
        public Item? Armor { get; set; }
        public Item? Head { get; set; }
        public Item? Boots { get; set; }
        public Item? Gloves { get; set; }
        public Item? Legs { get; set; }
        public Item? LeftBracelet { get; set; }
        public Item? Necklace { get; set; }
        public Item? RightBracelet { get; set; }
        public virtual Character? Character { get; set; }
        public virtual ICollection<Item>? LeftHandRings { get; set; }
        public virtual ICollection<Item>? RightHandRings { get; set; }

    }
}