using ROH.Domain.Effect;

namespace ROH.Domain.items;

public record Enchantment(long Id, long? Damage, long? Defense, string? Animation, string Name, EffectType Type)
{
    public virtual ICollection<ItemEnchantment>? Items { get; set; }
}