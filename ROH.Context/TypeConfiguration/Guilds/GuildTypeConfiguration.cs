using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Guilds;

namespace ROH.Context.TypeConfiguration.Guilds
{
    public class GuildTypeConfiguration : IEntityTypeConfiguration<Guild>
    {
        public void Configure(EntityTypeBuilder<Guild> builder)
        {
             builder.HasKey(g => g.Id);

             builder.Property(g => g.Guid).HasDefaultValueSql("gen_random_uuid()");

             builder.HasMany(g => g.Characters).WithOne(c => c.Guild).HasForeignKey(c => c.IdGuild);
             builder.HasMany(g => g.MembersPositions).WithOne(p => p.Guild).HasForeignKey(p => p.IdGuild);
        }
    }
}