using AutoMapper;

using ROH.Domain.Version;
using ROH.StandardModels.Version;

namespace ROH.Mapper.Version
{
    public class GameVersionMapping : Profile
    {
        public GameVersionMapping()
        {
            _ = CreateMap<GameVersion, GameVersionModel>().ReverseMap();
        }
    }
}