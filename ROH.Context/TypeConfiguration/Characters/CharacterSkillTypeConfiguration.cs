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
    public record CharacterSkillTypeConfiguration : IEntityTypeConfiguration<CharacterSkill>
    {
        public void Configure(EntityTypeBuilder<CharacterSkill> builder)
        {
            builder.HasKey(cs => cs.Id);

            builder.HasOne(cs => cs.Character).WithMany(c => c.Skills).HasForeignKey(cs => cs.IdCharacter);
            builder.HasOne(cs => cs.Skill).WithMany().HasForeignKey(cs => cs.IdSkill);
        }
    }
}
