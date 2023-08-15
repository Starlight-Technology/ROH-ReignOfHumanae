using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Version;

namespace ROH.Context.TypeConfiguration.Version
{
    public class GameVersionTypeConfiguration : IEntityTypeConfiguration<GameVersion>
    {
        public void Configure(EntityTypeBuilder<GameVersion> builder)
        {
            _ = builder.HasKey(g => g.Id);
        }
    }
}