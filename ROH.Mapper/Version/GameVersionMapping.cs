using AutoMapper;

using ROH.Domain.Version;
using ROH.StandardModels.Version;

namespace ROH.Mapper.Version
{
    public class GameVersionMapping : Profile
    {
        public GameVersionMapping() => CreateMap<GameVersion, GameVersionModel>().ReverseMap();
    }
}