//-----------------------------------------------------------------------
// <copyright file="GameVersionMapping.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using AutoMapper;

using ROH.Context.Version.Entities;
using ROH.StandardModels.Version;

namespace ROH.Mapping.Version;

public class GameVersionMapping : Profile
{
    public GameVersionMapping() => CreateMap<GameVersion, GameVersionModel>().ReverseMap();
}