using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Characters;

namespace ROH.Context.TypeConfiguration.Characters;

public record CharacterSkillTypeConfiguration : IEntityTypeConfiguration<CharacterSkill>
{
    public void Configure(EntityTypeBuilder<CharacterSkill> builder)
    {
        _ = builder.HasKey(cs => cs.Id);

        _ = builder.HasOne(cs => cs.Character).WithMany(c => c.Skills).HasForeignKey(cs => cs.IdCharacter);
        _ = builder.HasOne(cs => cs.Skill).WithMany().HasForeignKey(cs => cs.IdSkill);
    }
}