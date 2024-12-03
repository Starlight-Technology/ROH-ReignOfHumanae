//-----------------------------------------------------------------------
// <copyright file="GameFileMapping.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using AutoMapper;

using ROH.StandardModels.Version;

namespace ROH.Mapping.GameFile;

public class GameFileMapping : Profile
{
    public GameFileMapping() => CreateMap<Context.File.Entities.GameFile, GameVersionFileModel>().ReverseMap();
}
