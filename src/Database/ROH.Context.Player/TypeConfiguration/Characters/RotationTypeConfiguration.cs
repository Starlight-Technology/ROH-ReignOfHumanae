using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Context.Player.Entities.Characters;

namespace ROH.Context.Player.TypeConfiguration.Characters;

internal class RotationTypeConfiguration : IEntityTypeConfiguration<Rotation>
{
    public void Configure(EntityTypeBuilder<Rotation> builder)
    {
        builder.HasKey(p => p.Id);
    }
}