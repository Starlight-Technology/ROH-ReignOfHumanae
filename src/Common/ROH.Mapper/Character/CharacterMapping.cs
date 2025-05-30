using AutoMapper;

namespace ROH.Mapping.Character;

public class CharacterMapping : Profile
{
    public CharacterMapping()
    {
        CreateMap<Context.Player.Entities.Characters.Character, StandardModels.Character.CharacterModel>().ReverseMap();
        CreateMap<Context.Player.Entities.Characters.AttackStatus, StandardModels.Character.AttackStatus>().ReverseMap();
        CreateMap<Context.Player.Entities.Characters.DefenseStatus, StandardModels.Character.DefenseStatus>().ReverseMap();
        CreateMap<Context.Player.Entities.Characters.EquippedItems, StandardModels.Character.EquippedItems>().ReverseMap();
        CreateMap<Context.Player.Entities.Characters.HandRing, StandardModels.Character.HandRing>().ReverseMap();
        CreateMap<Context.Player.Entities.Characters.CharacterInventory, StandardModels.Character.CharacterInventory>().ReverseMap();
        CreateMap<Context.Player.Entities.Characters.CharacterSkill, StandardModels.Character.CharacterSkill>().ReverseMap();
        CreateMap<Context.Player.Entities.Characters.Skill, StandardModels.Character.Skill>().ReverseMap();
        CreateMap<Context.Player.Entities.Characters.PlayerPosition, StandardModels.Character.PlayerPositionModel>().ReverseMap();
        CreateMap<Context.Player.Entities.Characters.Position, StandardModels.Character.PositionModel>().ReverseMap();
        CreateMap<Context.Player.Entities.Characters.Rotation, StandardModels.Character.RotationModel>().ReverseMap();
        CreateMap<Context.Player.Entities.Characters.Status, StandardModels.Character.Status>().ReverseMap();
    }
}
