//-----------------------------------------------------------------------
// <copyright file="GameVersionModel.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace ROH.StandardModels.Version
{
    public class GameVersionModel
    {
        public GameVersionModel()
        {
        }

        public GameVersionListModel ToListModel() => new GameVersionListModel(
            Version,
            Release,
            Review,
            ReleaseDate,
            VersionDate,
            Guid);

        public Guid Guid { get; set; }

        public int Release { get; set; }

        public bool Released { get; set; }

        public int Review { get; set; }

        public int Version { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public DateTime VersionDate { get; set; }
    }
}