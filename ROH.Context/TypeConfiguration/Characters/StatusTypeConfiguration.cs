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
    public class StatusTypeConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.HasKey(s => s.IdCharacter);

            builder.HasOne(s => s.Character).WithOne(c => c.Status).HasForeignKey<Status>(s => s.IdCharacter);
        }
    }
}
