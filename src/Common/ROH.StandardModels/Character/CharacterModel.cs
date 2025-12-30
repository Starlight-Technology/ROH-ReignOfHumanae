using ROH.StandardModels.Character.PlayerStatus;
using ROH.StandardModels.Character.Position;

using System;
using System.Collections.Generic;

namespace ROH.StandardModels.Character
{
    public class CharacterModel
    {
        public CharacterModel()
        { }

        public CharacterModel(
            long id,
            Guid guidAccount,
            long? idGuild,
            long? idKingdom,
            Guid guid,
            string name,
            Race race)
        {
            Id = id;
            GuidAccount = guidAccount;
            IdGuild = idGuild;
            IdKingdom = idKingdom;
            Guid = guid;
            Name = name;
            Race = race;
        }

        public long Id { get; set; }
        public Guid GuidAccount { get; set; }
        public long? IdGuild { get; set; }
        public long? IdKingdom { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public Race Race { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual AttackStatus AttackStatus { get; set; }
        public virtual DefenseStatus DefenseStatus { get; set; }
        public virtual EquippedItems EquippedItems { get; set; }
        public virtual ICollection<CharacterInventory> Inventory { get; set; }
        public virtual ICollection<CharacterSkill> Skills { get; set; }
        public virtual Status Status { get; set; }
        public virtual PlayerPositionModel? PlayerPosition { get; set; }
    }
}