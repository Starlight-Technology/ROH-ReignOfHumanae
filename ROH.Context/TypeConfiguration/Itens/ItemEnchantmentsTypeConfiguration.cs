using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Itens;

namespace ROH.Context.TypeConfiguration.Itens
{
    public class ItemEnchantmentsTypeConfiguration : IEntityTypeConfiguration<ItemEnchantment>
    {
        public void Configure(EntityTypeBuilder<ItemEnchantment> builder)
        {
            _ = builder.HasKey(ie => ie.Id);

            _ = builder.HasOne(ie => ie.Item).WithMany(i => i.Enchantments).HasForeignKey(ie => ie.IdItem);
            _ = builder.HasOne(ie => ie.Enchantment).WithMany(i => i.Items).HasForeignKey(ie => ie.IdEnchantment);
        }
    }
}
