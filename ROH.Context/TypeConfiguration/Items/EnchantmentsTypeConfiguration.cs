using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.items;

namespace ROH.Context.TypeConfiguration.Items
{
    public class EnchantmentsTypeConfiguration : IEntityTypeConfiguration<Enchantment>
    {
        public void Configure(EntityTypeBuilder<Enchantment> builder)
        {
             builder.HasKey(e => e.Id);

             builder.HasMany(e => e.Items).WithOne(i => i.Enchantment).HasForeignKey(e => e.IdEnchantment);
        }
    }
}