using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Characters;

namespace ROH.Context.TypeConfiguration.Characters
{
    public class InventoryTypeConfiguration : IEntityTypeConfiguration<CharacterInventory>
    {
        public void Configure(EntityTypeBuilder<CharacterInventory> builder)
        {
             builder.HasKey(ci => ci.Id);

             builder.HasOne(ci => ci.Item).WithMany().HasForeignKey(ci => ci.IdItem);
             builder.HasOne(ci => ci.Character).WithMany(c => c.Inventory).HasForeignKey(ci => ci.IdCharacter);
        }
    }
}