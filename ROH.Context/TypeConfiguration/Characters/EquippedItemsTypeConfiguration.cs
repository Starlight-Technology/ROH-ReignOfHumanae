using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Characters;

namespace ROH.Context.TypeConfiguration.Characters
{
    public class EquippedItemsTypeConfiguration : IEntityTypeConfiguration<EquippedItems>
    {
        public void Configure(EntityTypeBuilder<EquippedItems> builder)
        {
            _ = builder.HasKey(e => e.IdCharacter);

            _ = builder.HasOne(e => e.Character).WithOne(c => c.EquippedItems).HasForeignKey<EquippedItems>(e => e.IdCharacter);

            _ = builder.HasOne(e => e.Boots).WithMany().HasForeignKey(e => e.IdBoots);
            _ = builder.HasOne(e => e.Head).WithMany().HasForeignKey(e => e.IdHead);
            _ = builder.HasOne(e => e.Armor).WithMany().HasForeignKey(e => e.IdArmor);
            _ = builder.HasOne(e => e.Gloves).WithMany().HasForeignKey(e => e.IdGloves);
            _ = builder.HasOne(e => e.Legs).WithMany().HasForeignKey(e => e.IdLegs);
            _ = builder.HasOne(e => e.Necklace).WithMany().HasForeignKey(e => e.IdNecklace);
            _ = builder.HasOne(e => e.LeftBracelet).WithMany().HasForeignKey(e => e.IdLeftBracelet);
            _ = builder.HasOne(e => e.RightBracelet).WithMany().HasForeignKey(e => e.IdRightBracelet);

            _ = builder.HasMany(e => e.LeftHandRings).WithOne(l => l.EquippedItems).HasForeignKey(l => l.IdEquippedItems);
            _ = builder.Ignore(e => e.LeftHandRings);
            _ = builder.HasMany(e => e.RightHandRings).WithOne(l => l.EquippedItems).HasForeignKey(l => l.IdEquippedItems);
        }
    }
}