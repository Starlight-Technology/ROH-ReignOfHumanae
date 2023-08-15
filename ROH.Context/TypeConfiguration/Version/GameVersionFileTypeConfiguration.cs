using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Version;

namespace ROH.Context.TypeConfiguration.Version
{
    public class GameVersionFileTypeConfiguration : IEntityTypeConfiguration<GameVersionFile>
    {
        public void Configure(EntityTypeBuilder<GameVersionFile> builder)
        {
            _ = builder.HasKey(f => f.Id);

            _ = builder.HasOne(f => f.GameVersion).WithMany().HasForeignKey(f => f.IdVersion);
        }
    }
}