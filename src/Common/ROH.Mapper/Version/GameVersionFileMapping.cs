//-----------------------------------------------------------------------
// <copyright file="GameVersionFileMapping.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using AutoMapper;

using ROH.Context.File.Entities;
using ROH.StandardModels.Version;

namespace ROH.Mapping.Version;

public class GameVersionFileMapping : Profile
{
    public GameVersionFileMapping() => CreateMap<GameVersionFile, GameVersionFileModel>().ReverseMap();
}