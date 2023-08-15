using Microsoft.EntityFrameworkCore;

using ROH.Context.TypeConfiguration.Accounts;
using ROH.Context.TypeConfiguration.Characters;
using ROH.Context.TypeConfiguration.Guilds;
using ROH.Context.TypeConfiguration.items;
using ROH.Context.TypeConfiguration.Kingdoms;
using ROH.Context.TypeConfiguration.Version;
using ROH.Domain.Accounts;
using ROH.Domain.Characters;
using ROH.Domain.Guilds;
using ROH.Domain.items;
using ROH.Domain.Kingdoms;
using ROH.Domain.Version;
using ROH.Interfaces;

namespace ROH.Context.PostgresSQLContext
{
    public class SqlContext : DbContext, ISqlContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<AttackStatus> AttackStatuses { get; set; }

        public DbSet<Character> Characters { get; set; }

        public DbSet<DefenseStatus> DefenseStatuses { get; set; }

        public DbSet<EquippedItems> EquippedItems { get; set; }

        public DbSet<Skill> Skills { get; set; }

        public DbSet<Status> Statuses { get; set; }

        public DbSet<Guild> Guilds { get; set; }

        public DbSet<MembersPosition> MembersPositions { get; set; }

        public DbSet<Enchantment> Enchantments { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Kingdom> Kingdoms { get; set; }

        public DbSet<KingdomRelation> KingdomRelations { get; set; }

        public DbSet<CharacterSkill> CharacterSkills { get; set; }

        public DbSet<HandRing> RingsEquipped { get; set; }

        public DbSet<CharacterInventory> CharacterInventory { get; set; }

        public DbSet<ItemEnchantment> ItemEnchantments { get; set; }

        public DbSet<Champion> Champions { get; set; }

        public DbSet<GameVersion> GameVersions { get; set; }

        public DbSet<GameVersionFile> GameVersionFiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _ = optionsBuilder.UseNpgsql("Host=192.168.0.37;Port=5432;Database=ROH;Username=teste;Password=Teste123;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _ = modelBuilder.ApplyConfiguration(new AccountTypeConfiguration());
            _ = modelBuilder.ApplyConfiguration(new UserTypeConfiguration());
            _ = modelBuilder.ApplyConfiguration(new AttackStatusTypeConfiguration());
            _ = modelBuilder.ApplyConfiguration(new CharacterSkillTypeConfiguration());
            _ = modelBuilder.ApplyConfiguration(new CharacterTypeConfiguration());
            _ = modelBuilder.ApplyConfiguration(new DefenseStatusTypeConfiguration());
            _ = modelBuilder.ApplyConfiguration(new EquippedItemsTypeConfiguration());
            _ = modelBuilder.ApplyConfiguration(new HandRingTypeConfiguration());
            _ = modelBuilder.ApplyConfiguration(new InventoryTypeConfiguration());
            _ = modelBuilder.ApplyConfiguration(new SkillTypeConfiguration());
            _ = modelBuilder.ApplyConfiguration(new StatusTypeConfiguration());
            _ = modelBuilder.ApplyConfiguration(new GuildTypeConfiguration());
            _ = modelBuilder.ApplyConfiguration(new MembersPositionTypeConfiguration());
            _ = modelBuilder.ApplyConfiguration(new EnchantmentsTypeConfiguration());
            _ = modelBuilder.ApplyConfiguration(new ItemEnchantmentsTypeConfiguration());
            _ = modelBuilder.ApplyConfiguration(new ItemTypeConfiguration());
            _ = modelBuilder.ApplyConfiguration(new ChampionTypeConfiguration());
            _ = modelBuilder.ApplyConfiguration(new KingdomTypeConfiguration());
            _ = modelBuilder.ApplyConfiguration(new RelationTypeConfiguration());
            _ = modelBuilder.ApplyConfiguration(new GameVersionTypeConfiguration());
            _ = modelBuilder.ApplyConfiguration(new GameVersionFileTypeConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}