using Microsoft.EntityFrameworkCore;

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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql("Data Source=192.168.0.37,1433;Initial Catalog=ROH;User ID=teste;Password=Teste123;");


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            base.OnModelCreating(modelBuilder);
        }


        public override int SaveChanges() => base.SaveChanges();

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
