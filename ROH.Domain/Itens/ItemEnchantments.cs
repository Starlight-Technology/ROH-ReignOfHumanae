namespace ROH.Domain.Itens
{
    public record ItemEnchantments(long Id, long IdItem, long IdEnchantment, Item Item, Enchantment Enchantment)
    {
    }
}