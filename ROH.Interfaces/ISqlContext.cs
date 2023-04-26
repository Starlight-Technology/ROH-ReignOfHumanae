using Microsoft.EntityFrameworkCore;

using ROH.Domain.Accounts;
using ROH.Domain.Characters;
using ROH.Domain.Guilds;
using ROH.Domain.Itens;
using ROH.Domain.Kingdoms;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Interfaces
{
    public interface ISqlContext
    {
        DbSet<User> Users { get; }
        DbSet<Account> Accounts { get; }
        DbSet<AttackStatus> AttackStatuses { get; }
        DbSet<Character> Characters { get; }
        DbSet<DefenseStatus> DefenseStatuses { get; }
        DbSet<EquipedItens> EquipedItens { get; }
        DbSet<Skill> Skills { get; }
        DbSet<Status> Statuses { get; }
        DbSet<Guild> Guilds { get; }
        DbSet<MembersPosition> MembersPositions { get; }
        DbSet<Enchantment> Enchantments { get; }
        DbSet<Item> Items { get; }
        DbSet<Kingdom> Kingdoms { get; }
        DbSet<KingdomRelation> KingdomRelations { get; }

        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
