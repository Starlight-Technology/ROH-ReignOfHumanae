namespace ROH.Domain.Itens
{
    public record ItemEnchantment(long Id, long IdItem, long IdEnchantment)
    {
        public virtual Item? Item { get; set; }
        public virtual Enchantment? Enchantment { get; set; }
    }
}