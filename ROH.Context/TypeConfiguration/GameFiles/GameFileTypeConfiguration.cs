using Microsoft.EntityFrameworkCore;

using ROH.Domain.GameFiles;

namespace ROH.Context.TypeConfiguration.GameFiles;
public class GameFileTypeConfiguration : IEntityTypeConfiguration<GameFile>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<GameFile> builder)
    {
        _ = builder.HasKey(f => f.Id);

        _ = builder.Property(g => g.Guid).HasDefaultValueSql("gen_random_uuid()");
    }
}
