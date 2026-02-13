//-----------------------------------------------------------------------
// <copyright file="GameVersionListModel.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace ROH.StandardModels.Version
{
    public class GameVersionListModel
    {
        public GameVersionListModel(
            int version,
            int release,
            int review,
            DateTime? releaseDate,
            DateTime versionDate,
            Guid guid)
        {
            Version = version;
            Release = release;
            Review = review;
            VersionDate = versionDate;
            ReleaseDate = releaseDate;
            DetailsLink = $"/Manager/Version/VersionDetails/{guid}";
        }

        public int Release { get; set; }

        public int Review { get; set; }

        public int Version { get; set; }

        public DateTime VersionDate { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public string DetailsLink { get; set; }
    }
}