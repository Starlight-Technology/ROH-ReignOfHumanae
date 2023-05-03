using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Characters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Context.TypeConfiguration.Characters
{
    public class AttackStatusTypeConfiguration : IEntityTypeConfiguration<AttackStatus>
    {
        public void Configure(EntityTypeBuilder<AttackStatus> builder)
        {
            builder.HasKey(a => a.IdCharacter);

            builder.HasOne(a => a.Character).WithOne(c => c.AttackStatus).HasForeignKey<AttackStatus>(a => a.IdCharacter);
        }
    }
}
