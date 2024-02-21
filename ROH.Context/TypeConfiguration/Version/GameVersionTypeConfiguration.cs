using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Version;

namespace ROH.Context.TypeConfiguration.Version
{
    public class GameVersionTypeConfiguration : IEntityTypeConfiguration<GameVersion>
    {
        public void Configure(EntityTypeBuilder<GameVersion> builder)
        {
             builder.HasKey(g => g.Id);

             builder.Property(g => g.Guid).HasDefaultValueSql("gen_random_uuid()");
        }
    }
}