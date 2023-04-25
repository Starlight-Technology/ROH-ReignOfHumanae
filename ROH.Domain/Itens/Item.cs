namespace ROH.Domain.Itens
{
    public record Item(long Id, int? Attack, int? Defense, int Weight, string? Name, string? Descricao, string? Sprite, string? File, string? Format, ICollection<Enchantment>? Enchantments);
}
