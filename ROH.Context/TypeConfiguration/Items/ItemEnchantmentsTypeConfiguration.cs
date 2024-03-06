using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.items;

namespace ROH.Context.TypeConfiguration.Items
{
    public class ItemEnchantmentsTypeConfiguration : IEntityTypeConfiguration<ItemEnchantment>
    {
        public void Configure(EntityTypeBuilder<ItemEnchantment> builder)
        {
             builder.HasKey(ie => ie.Id);

             builder.HasOne(ie => ie.Item).WithMany(i => i.Enchantments).HasForeignKey(ie => ie.IdItem);
             builder.HasOne(ie => ie.Enchantment).WithMany(i => i.Items).HasForeignKey(ie => ie.IdEnchantment);
        }
    }
}