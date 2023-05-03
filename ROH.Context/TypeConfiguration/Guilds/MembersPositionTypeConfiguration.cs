using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Guilds;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Context.TypeConfiguration.Guilds
{
    public class MembersPositionTypeConfiguration : IEntityTypeConfiguration<MembersPosition>
    {
        public void Configure(EntityTypeBuilder<MembersPosition> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.Guild).WithMany(g => g.Positions).HasForeignKey(p => p.IdGuild);
            builder.HasOne(p => p.Character).WithOne().HasForeignKey<MembersPosition>(p => p.IdCharacter);
        }
    }
}
