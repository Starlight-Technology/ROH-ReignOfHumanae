//-----------------------------------------------------------------------
// <copyright file="GameVersionFileModel.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.StandardModels.File;

using System;

namespace ROH.StandardModels.Version
{
    public class GameVersionFileModel
    {
        public GameFileModel ToFileModel() => new GameFileModel(Name, Format, Content, Size, Active);

        public GameVersionFileListModel ToListModel() => new GameVersionFileListModel(Name, Size, Guid);

        public bool Active { get; set; } = false;

        public byte[]? Content { get; set; } = null;

        public string Format { get; set; } = string.Empty;

        public virtual GameVersionModel? GameVersion { get; set; }

        public Guid Guid { get; set; } = Guid.Empty;

        public string Name { get; set; } = string.Empty;

        public string Path { get; set; } = string.Empty;

        public long Size { get; set; }
    }
}