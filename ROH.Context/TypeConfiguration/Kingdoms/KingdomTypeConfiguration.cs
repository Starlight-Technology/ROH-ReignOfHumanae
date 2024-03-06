using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Kingdoms;

namespace ROH.Context.TypeConfiguration.Kingdoms
{
    public class KingdomTypeConfiguration : IEntityTypeConfiguration<Kingdom>
    {
        public void Configure(EntityTypeBuilder<Kingdom> builder)
        {
             builder.HasKey(k => k.Id);

             builder.HasMany(k => k.Citizens).WithOne(c => c.Kingdom).HasForeignKey(c => c.IdKingdom);
             builder.HasMany(k => k.Champions).WithOne(c => c.Kingdom).HasForeignKey(c => c.IdKingdom);

             builder.HasOne(k => k.Ruler).WithOne().HasForeignKey<Kingdom>(k => k.IdRuler);
        }
    }
}