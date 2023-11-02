namespace ROH.Domain.items
{
    public record Item(long Id, Guid Guid, int? Attack, int? Defense, int Weight, string? Name, string? Descricao, string? Sprite, string? File, string? Format)
    {
        public virtual ICollection<ItemEnchantment>? Enchantments { get; set; }
    }
}