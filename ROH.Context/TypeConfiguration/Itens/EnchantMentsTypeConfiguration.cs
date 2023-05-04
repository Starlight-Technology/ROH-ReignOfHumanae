using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Itens;

namespace ROH.Context.TypeConfiguration.Itens
{
    public class EnchantmentsTypeConfiguration : IEntityTypeConfiguration<Enchantment>
    {
        public void Configure(EntityTypeBuilder<Enchantment> builder)
        {
            _ = builder.HasKey(e => e.Id);

            _ = builder.HasMany(e => e.Items).WithOne(i => i.Enchantment).HasForeignKey(e => e.IdEnchantment);
        }
    }
}
