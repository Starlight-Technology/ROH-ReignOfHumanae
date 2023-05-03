using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Kingdoms;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Context.TypeConfiguration.Kingdoms
{
    public class RelationTypeConfiguration : IEntityTypeConfiguration<KingdomRelation>
    {
        public void Configure(EntityTypeBuilder<KingdomRelation> builder)
        {
            builder.HasKey(k => k.Id);

            builder.HasOne(k => k.Kingdom).WithMany(k => k.KingdomRelations).HasForeignKey(k => k.IdKingdom);
            builder.HasOne(k => k.Kingdom2).WithMany(k => k.KingdomRelations).HasForeignKey(k => k.IdKingdom2);
        }
    }
}
