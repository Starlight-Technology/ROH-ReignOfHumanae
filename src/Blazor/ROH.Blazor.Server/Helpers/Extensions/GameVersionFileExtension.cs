//-----------------------------------------------------------------------
// <copyright file="GameVersionFileExtension.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.StandardModels.Version;

namespace ROH.Blazor.Server.Helpers.Extensions;

public static class GameVersionFileExtension
{
    public static List<GameVersionFileListModel> ToListModel(this List<GameVersionFileModel> model) => model.Select(
        file => file.ToListModel())
        .ToList();
}
