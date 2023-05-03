using Microsoft.EntityFrameworkCore;

using ROH.Context.TypeConfiguration.Accounts;
using ROH.Context.TypeConfiguration.Characters;
using ROH.Context.TypeConfiguration.Guilds;
using ROH.Context.TypeConfiguration.Itens;
using ROH.Context.TypeConfiguration.Kingdoms;
using ROH.Domain.Accounts;
using ROH.Domain.Characters;
using ROH.Domain.Guilds;
using ROH.Domain.Itens;
using ROH.Domain.Kingdoms;
using ROH.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Context.PostgreSQLContext
{
    public class SqlContext : DbContext, ISqlContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<AttackStatus> AttackStatuses { get; set; }

        public DbSet<Character> Characters { get; set; }

        public DbSet<DefenseStatus> DefenseStatuses { get; set; }

        public DbSet<EquippedItens> EquipedItens { get; set; }

        public DbSet<Skill> Skills { get; set; }

        public DbSet<Status> Statuses { get; set; }

        public DbSet<Guild> Guilds { get; set; }

        public DbSet<MembersPosition> MembersPositions { get; set; }

        public DbSet<Enchantment> Enchantments { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Kingdom> Kingdoms { get; set; }

        public DbSet<KingdomRelation> KingdomRelations { get; set; }

        public DbSet<CharacterSkill> characterSkills { get; set; }

        public DbSet<HandRing> RingsEquipped { get; set; }

        public DbSet<CharacterInventory> CharacterInventories { get; set; }

        public DbSet<ItemEnchantment> ItemEnchantments { get; set; }

        public DbSet<Champion> Champions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql("Host=192.168.0.37;Port=5432;Database=ROH;Username=teste;Password=Teste123;");


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AttackStatusTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CharacterSkillTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CharacterTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DefenseStatusTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EquippedItensTypeConfiguration());
            modelBuilder.ApplyConfiguration(new HandRingTypeConfiguration());
            modelBuilder.ApplyConfiguration(new InventoryTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SkillTypeConfiguration());
            modelBuilder.ApplyConfiguration(new StatusTypeConfiguration());
            modelBuilder.ApplyConfiguration(new GuildTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MembersPositionTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EnchantmentsTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ItemEnchantmentsTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ItemTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ChampionTypeConfiguration());
            modelBuilder.ApplyConfiguration(new KingdomTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RelationTypeConfiguration());

            base.OnModelCreating(modelBuilder);
        }


        public override int SaveChanges() => base.SaveChanges();

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
