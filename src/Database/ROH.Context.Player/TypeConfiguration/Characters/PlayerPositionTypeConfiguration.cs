using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Context.Player.Entities.Characters;

namespace ROH.Context.Player.TypeConfiguration.Characters;

public class PlayerPositionTypeConfiguration : IEntityTypeConfiguration<PlayerPosition>
{
    void IEntityTypeConfiguration<PlayerPosition>.Configure(EntityTypeBuilder<PlayerPosition> builder)
    {
        _ = builder.HasKey(p => p.Id);
        builder.HasOne(p => p.Position).WithMany().HasForeignKey(p => p.PositionId);
        builder.HasOne(p => p.Rotation).WithMany().HasForeignKey(p => p.RotationId);
    }
}