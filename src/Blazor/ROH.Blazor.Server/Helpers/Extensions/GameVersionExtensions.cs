//-----------------------------------------------------------------------
// <copyright file="GameVersionExtensions.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.StandardModels.Version;

namespace ROH.Blazor.Server.Helpers.Extensions;

public static class GameVersionExtensions
{
    public static List<GameVersionListModel> ToListModel(this List<GameVersionModel> gameVersions) => gameVersions.Select(
        version => version.ToListModel())
        .ToList();
}
