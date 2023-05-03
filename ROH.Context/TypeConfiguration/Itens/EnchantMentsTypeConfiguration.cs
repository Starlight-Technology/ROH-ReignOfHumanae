using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Itens;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Context.TypeConfiguration.Itens
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
