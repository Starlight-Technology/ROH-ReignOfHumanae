using AutoMapper;

using ROH.Domain.Version;
using ROH.StandardModels.Version;

namespace ROH.Mapper.Version;

public class GameVersionFileMapping : Profile
{
    public GameVersionFileMapping() => CreateMap<GameVersionFile, GameVersionFileModel>().ReverseMap();
}