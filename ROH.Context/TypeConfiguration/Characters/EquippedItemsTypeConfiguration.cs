using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Characters;

namespace ROH.Context.TypeConfiguration.Characters
{
    public class EquippedItemsTypeConfiguration : IEntityTypeConfiguration<EquippedItems>
    {
        public void Configure(EntityTypeBuilder<EquippedItems> builder)
        {
             builder.HasKey(e => e.IdCharacter);

             builder.HasOne(e => e.Character).WithOne(c => c.EquippedItems).HasForeignKey<EquippedItems>(e => e.IdCharacter);

             builder.HasOne(e => e.Boots).WithMany().HasForeignKey(e => e.IdBoots);
             builder.HasOne(e => e.Head).WithMany().HasForeignKey(e => e.IdHead);
             builder.HasOne(e => e.Armor).WithMany().HasForeignKey(e => e.IdArmor);
             builder.HasOne(e => e.Gloves).WithMany().HasForeignKey(e => e.IdGloves);
             builder.HasOne(e => e.Legs).WithMany().HasForeignKey(e => e.IdLegs);
             builder.HasOne(e => e.Necklace).WithMany().HasForeignKey(e => e.IdNecklace);
             builder.HasOne(e => e.LeftBracelet).WithMany().HasForeignKey(e => e.IdLeftBracelet);
             builder.HasOne(e => e.RightBracelet).WithMany().HasForeignKey(e => e.IdRightBracelet);

             builder.HasMany(e => e.LeftHandRings).WithOne(l => l.EquippedItems).HasForeignKey(l => l.IdEquippedItems);
             builder.Ignore(e => e.LeftHandRings);
             builder.HasMany(e => e.RightHandRings).WithOne(l => l.EquippedItems).HasForeignKey(l => l.IdEquippedItems);
        }
    }
}