using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Characters;

namespace ROH.Context.TypeConfiguration.Characters
{
    public class CharacterTypeConfiguration : IEntityTypeConfiguration<Character>
    {
        public void Configure(EntityTypeBuilder<Character> builder)
        {
            _ = builder.HasKey(c => c.Id);

            _ = builder.Property(g => g.Guid).HasDefaultValueSql("gen_random_uuid()");

            _ = builder.HasOne(c => c.Account).WithMany(a => a.Characters).HasForeignKey(c => c.IdAccount);

            _ = builder.HasOne(c => c.AttackStatus).WithOne(a => a.Character).HasForeignKey<AttackStatus>(a => a.IdCharacter);
            _ = builder.HasOne(c => c.DefenseStatus).WithOne(a => a.Character).HasForeignKey<DefenseStatus>(a => a.IdCharacter);
            _ = builder.HasOne(c => c.Status).WithOne(a => a.Character).HasForeignKey<Status>(a => a.IdCharacter);
            _ = builder.HasOne(c => c.EquippedItems).WithOne(a => a.Character).HasForeignKey<EquippedItems>(a => a.IdCharacter);

            _ = builder.HasMany(c => c.Skills).WithOne(s => s.Character).HasForeignKey(c => c.IdCharacter);
            _ = builder.HasMany(c => c.Inventory).WithOne(i => i.Character).HasForeignKey(i => i.IdCharacter);
        }
    }
}