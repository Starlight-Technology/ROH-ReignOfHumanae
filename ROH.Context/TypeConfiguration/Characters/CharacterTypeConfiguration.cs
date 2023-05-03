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
    public class CharacterTypeConfiguration : IEntityTypeConfiguration<Character>
    {
        public void Configure(EntityTypeBuilder<Character> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.Account).WithMany(a => a.Characters).HasForeignKey(c => c.IdAccount);

            builder.HasOne(c => c.AttackStatus).WithOne(a => a.Character).HasForeignKey<AttackStatus>(a => a.IdCharacter);
            builder.HasOne(c => c.DefenseStatus).WithOne(a => a.Character).HasForeignKey<DefenseStatus>(a => a.IdCharacter);
            builder.HasOne(c => c.Status).WithOne(a => a.Character).HasForeignKey<Status>(a => a.IdCharacter);
            builder.HasOne(c => c.EquippedItens).WithOne(a => a.Character).HasForeignKey<EquippedItens>(a => a.IdCharacter);

            builder.HasMany(c => c.Skills).WithOne(s => s.Character).HasForeignKey(c => c.IdCharacter);
            builder.HasMany(c => c.Inventory).WithOne(i => i.Character).HasForeignKey(i => i.IdCharacter);
        }
    }
}
