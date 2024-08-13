using AutoMapper;

using ROH.StandardModels.Version;

namespace ROH.Mapping.GameFile;
public class GameFileMapping : Profile
{
    public GameFileMapping() => CreateMap<Domain.GameFiles.GameFile, GameVersionFileModel>().ReverseMap();
}
