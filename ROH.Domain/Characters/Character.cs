using ROH.Domain.Accounts;
using ROH.Domain.Guilds;
using ROH.Domain.Itens;
using ROH.Domain.Kingdoms;

namespace ROH.Domain.Characters
{
    public record Character(long Id,
                            long IdAccount,
                            long? IdGuild,
                            long IdKingdom,
                            string? Name,
                            Race Race,
                            AttackStatus AttackStatus,
                            Account Account,
                            DefenseStatus DefenseStatus,
                            EquippedItens EquippedItens,
                            Status Status,
                            Guild? Guild,
                            Kingdom Kingdom,
                            ICollection<CharacterInventory> Inventory,
                            ICollection<CharacterSkill> Skills)
    {
        private DateTime dateCreated;

        public Character(long id, long idAccount, long? idGuild, long idKingdom, string? name, Race race, AttackStatus attackStatus, Account account, DefenseStatus defenseStatus, EquippedItens equippedItens, Status status, Guild? guild, Kingdom kingdom, ICollection<CharacterInventory> inventory, ICollection<CharacterSkill> skills, DateTime dateCreated) : this(id, idAccount, idGuild, idKingdom, name, race, attackStatus, account, defenseStatus, equippedItens, status, guild, kingdom, inventory, skills)
        {
            this.dateCreated = dateCreated;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Blocker Code Smell", "S3237:\"value\" parameters should be used", Justification = "<Is defined on set.>")]
        public DateTime DateCreated { get => dateCreated; private set => dateCreated = DateTime.Now; }
    }
}
