using ROH.Domain.Guilds;
using ROH.Domain.Itens;

namespace ROH.Domain.Characters
{
    public class Character
    {
        private DateTime dateCreated;

        public Character(DateTime dateCreated, Race race, AttackStatus attackStatus, DefenseStatus defenseStatus, EquipedItens equipedItens, Status status, ICollection<Item> inventory, ICollection<Skill> skills)
        {
            this.dateCreated = dateCreated;
            Race = race;
            AttackStatus = attackStatus ?? throw new ArgumentNullException(nameof(attackStatus));
            DefenseStatus = defenseStatus ?? throw new ArgumentNullException(nameof(defenseStatus));
            EquipedItens = equipedItens ?? throw new ArgumentNullException(nameof(equipedItens));
            Status = status ?? throw new ArgumentNullException(nameof(status));
            Inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            Skills = skills ?? throw new ArgumentNullException(nameof(skills));
        }

        public long Id { get; set; }
        public long IdAccount { get; set; }
        public string? Name { get; set; }
        public Race Race { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Blocker Code Smell", "S3237:\"value\" parameters should be used", Justification = "<Is defined on set.>")]
        public DateTime DateCreated { get => dateCreated; private set => dateCreated = DateTime.Now; }
        public virtual AttackStatus AttackStatus { get; set; }
        public virtual DefenseStatus DefenseStatus { get; set; }
        public virtual EquipedItens EquipedItens { get; set; }
        public virtual Status Status { get; set; }
        public virtual Guild? Guild { get; set; }
        public virtual ICollection<Item> Inventory { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }


    }
}
