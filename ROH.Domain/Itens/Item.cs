namespace ROH.Domain.Itens
{
    public class Item
    {
        public long Id { get; set; }
        public int? Attack { get; set; }
        public int? Defense { get; set; }
        public int Weight { get; set; }
        public string? Name { get; set; }
        public string? Descricao { get; set; }
        public string? Sprite { get; set; }
        public string? File { get; set; }
        public string? Format { get; set; }
        public virtual ICollection<Enchantment>? Enchantments { get; set; }

    }
}
