using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Characters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Context.TypeConfiguration.Characters
{
    public class HandRingTypeConfiguration : IEntityTypeConfiguration<HandRing>
    {
        public void Configure(EntityTypeBuilder<HandRing> builder)
        {
            builder.HasKey(r => r.Id);

            builder.HasOne(r => r.Item).WithMany().HasForeignKey(r => r.IdItem);
        }
    }
}
