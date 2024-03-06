using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Characters;

namespace ROH.Context.TypeConfiguration.Characters
{
    public class DefenseStatusTypeConfiguration : IEntityTypeConfiguration<DefenseStatus>
    {
        public void Configure(EntityTypeBuilder<DefenseStatus> builder)
        {
            _ = builder.HasKey(ds => ds.IdCharacter);

            _ = builder.HasOne(a => a.Character).WithOne(c => c.DefenseStatus).HasForeignKey<DefenseStatus>(a => a.IdCharacter);
        }
    }
}