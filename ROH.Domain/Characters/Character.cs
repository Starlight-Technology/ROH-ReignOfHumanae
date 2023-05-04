using ROH.Domain.Accounts;
using ROH.Domain.Guilds;
using ROH.Domain.Kingdoms;

namespace ROH.Domain.Characters
{
    public record Character(long Id,
                            long IdAccount,
                            long? IdGuild,
                            long IdKingdom,
                            string? Name,
                            Race Race)
    {
        private DateTime dateCreated;

        public Character(long id, long idAccount, long? idGuild, long idKingdom, string? name, Race race, DateTime dateCreated) : this(id, idAccount, idGuild, idKingdom, name, race)
        {
            this.dateCreated = dateCreated;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Blocker Code Smell", "S3237:\"value\" parameters should be used", Justification = "<Is defined on set.>")]
        public DateTime DateCreated { get => dateCreated; private set => dateCreated = DateTime.Now; }

        public virtual AttackStatus AttackStatus { get; set; }
        public virtual Account Account { get; set; }
        public virtual DefenseStatus DefenseStatus { get; set; }
        public virtual EquippedItens EquippedItens { get; set; }
        public virtual Status Status { get; set; }
        public virtual Guild? Guild { get; set; }
        public virtual Kingdom Kingdom { get; set; }
        public virtual ICollection<CharacterInventory> Inventory { get; set; }
        public virtual ICollection<CharacterSkill> Skills { get; set; }
    }
}
