using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Characters;

namespace ROH.Context.TypeConfiguration.Characters
{
    public class StatusTypeConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            _ = builder.HasKey(s => s.IdCharacter);

            _ = builder.HasOne(s => s.Character).WithOne(c => c.Status).HasForeignKey<Status>(s => s.IdCharacter);
        }
    }
}
