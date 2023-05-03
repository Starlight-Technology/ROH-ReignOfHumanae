using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Kingdoms;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Context.TypeConfiguration.Kingdoms
{
    public class ChampionTypeConfiguration : IEntityTypeConfiguration<Champion>
    {
        public void Configure(EntityTypeBuilder<Champion> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.Character).WithMany().HasForeignKey(c => c.IdCharacter);
            builder.HasOne(c => c.Kingdom).WithMany(k => k.Champions).HasForeignKey(c => c.IdKingdom);
        }
    }
}
