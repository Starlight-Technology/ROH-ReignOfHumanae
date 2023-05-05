using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Version;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Context.TypeConfiguration.Version
{
    public class GameVersionTypeConfiguration : IEntityTypeConfiguration<GameVersion>
    {
        public void Configure(EntityTypeBuilder<GameVersion> builder)
        {
            builder.HasKey(g => g.Id);
        }
    }
}
