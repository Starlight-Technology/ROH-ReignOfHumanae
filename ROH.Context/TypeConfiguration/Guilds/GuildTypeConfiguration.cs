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
    public class GuildTypeConfiguration : IEntityTypeConfiguration<Guild>
    {
        public void Configure(EntityTypeBuilder<Guild> builder)
        {
            builder.HasKey(g => g.Id);

            builder.HasMany(g => g.Characters).WithOne(c => c.Guild).HasForeignKey(c => c.IdGuild);
            builder.HasMany(g => g.Positions).WithOne(p => p.Guild).HasForeignKey(p => p.IdGuild);
        }
    }
}
