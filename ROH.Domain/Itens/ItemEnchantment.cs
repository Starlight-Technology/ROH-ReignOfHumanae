namespace ROH.Domain.Itens
{
    public record ItemEnchantment(long Id, long IdItem, long IdEnchantment, Item Item, Enchantment Enchantment)
    {
    }
}