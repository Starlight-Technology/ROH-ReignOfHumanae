using ROH.Domain.Effect;

namespace ROH.Domain.Itens
{
    public record Enchantment(long Id, long? Damage, long? Defense, string? Animation, string Name, EffectType Type, ICollection<ItemEnchantment> Items)
    {
    }
}