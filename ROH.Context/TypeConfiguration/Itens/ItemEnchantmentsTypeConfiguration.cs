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
    public class ItemEnchantmentsTypeConfiguration : IEntityTypeConfiguration<ItemEnchantments>
    {
        public void Configure(EntityTypeBuilder<ItemEnchantments> builder)
        {
            builder.HasKey(ie => ie.Id);

            builder.HasOne(ie => ie.Item).WithMany(i => i.Enchantments).HasForeignKey(ie => ie.IdItem);
            builder.HasOne(ie => ie.Enchantment).WithMany(i => i.Items).HasForeignKey(ie => ie.IdEnchantment);
        }
    }
}
