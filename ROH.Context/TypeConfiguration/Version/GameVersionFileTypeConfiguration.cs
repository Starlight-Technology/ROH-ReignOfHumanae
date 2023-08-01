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
    public class GameVersionFileTypeConfiguration : IEntityTypeConfiguration<GameVersionFile>
    {
        public void Configure(EntityTypeBuilder<GameVersionFile> builder)
        {
            builder.HasKey(f => f.Id);

            builder.HasOne(f => f.GameVersion).WithMany().HasForeignKey(f => f.IdVersion);
        }
    }
}
