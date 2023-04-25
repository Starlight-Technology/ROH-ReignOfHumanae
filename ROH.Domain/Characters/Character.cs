using ROH.Domain.Guilds;
using ROH.Domain.Itens;

namespace ROH.Domain.Characters
{
    public record Character(long Id, long IdAccount, string? Name, Race Race, AttackStatus AttackStatus, DefenseStatus DefenseStatus, EquipedItens EquipedItens, Status Status, Guild? Guild, ICollection<Item> Inventory, ICollection<Skill> Skills)
    {
        private DateTime dateCreated;

        public Character(DateTime dateCreated, Race race, AttackStatus attackStatus, DefenseStatus defenseStatus, EquipedItens equipedItens, Status status, ICollection<Item> inventory, ICollection<Skill> skills) : this(default, default, null, race, attackStatus ?? throw new ArgumentNullException(nameof(attackStatus)), defenseStatus ?? throw new ArgumentNullException(nameof(defenseStatus)), equipedItens ?? throw new ArgumentNullException(nameof(equipedItens)), status ?? throw new ArgumentNullException(nameof(status)), null, inventory ?? throw new ArgumentNullException(nameof(inventory)), skills ?? throw new ArgumentNullException(nameof(skills)))
        {
            this.dateCreated = dateCreated;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Blocker Code Smell", "S3237:\"value\" parameters should be used", Justification = "<Is defined on set.>")]
        public DateTime DateCreated { get => dateCreated; private set => dateCreated = DateTime.Now; }
    }
}
