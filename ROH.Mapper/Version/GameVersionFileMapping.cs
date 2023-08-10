using AutoMapper;

using ROH.Domain.Version;
using ROH.StandardModels.Version;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Mapper.Version
{
    public class GameVersionFileMapping : Profile
    {
        public GameVersionFileMapping()
        {
            CreateMap<GameVersionFile, GameVersionFileModel>().ReverseMap();
        }
    }
}
