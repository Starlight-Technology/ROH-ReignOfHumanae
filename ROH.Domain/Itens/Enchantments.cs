namespace ROH.Domain.Itens
{
    public record Enchantment(long Id, long? Damage, long? Defense, string? Animation, string Name, EnchantmentType Type)
    {
    }
}