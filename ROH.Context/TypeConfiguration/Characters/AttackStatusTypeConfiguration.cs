using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Characters;

namespace ROH.Context.TypeConfiguration.Characters
{
    public class AttackStatusTypeConfiguration : IEntityTypeConfiguration<AttackStatus>
    {
        public void Configure(EntityTypeBuilder<AttackStatus> builder)
        {
            _ = builder.HasKey(a => a.IdCharacter);

            _ = builder.HasOne(a => a.Character).WithOne(c => c.AttackStatus).HasForeignKey<AttackStatus>(a => a.IdCharacter);
        }
    }
}