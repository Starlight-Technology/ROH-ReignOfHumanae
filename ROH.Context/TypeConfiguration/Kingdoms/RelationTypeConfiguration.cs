using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Kingdoms;

namespace ROH.Context.TypeConfiguration.Kingdoms;

public class RelationTypeConfiguration : IEntityTypeConfiguration<KingdomRelation>
{
    public void Configure(EntityTypeBuilder<KingdomRelation> builder)
    {
        _ = builder.HasKey(k => k.Id);

        _ = builder.HasOne(k => k.Kingdom).WithMany(k => k.KingdomRelations).HasForeignKey(k => k.IdKingdom);
        _ = builder.Ignore(k => k.Kingdom);

        _ = builder.HasOne(k => k.Kingdom2).WithMany(k => k.KingdomRelations).HasForeignKey(k => k.IdKingdom2);
    }
}