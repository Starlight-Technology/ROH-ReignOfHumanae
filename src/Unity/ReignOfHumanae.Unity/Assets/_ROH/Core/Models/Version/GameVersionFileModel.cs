//-----------------------------------------------------------------------
// <copyright file="GameVersionFileModel.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace Assets.Scripts.Models.Version
{
    [Serializable]
    public record GameVersionFileModel
    {
        public bool Active { get; set; }

        public byte[] Content { get; set; }

        public string Format { get; set; }

        public virtual GameVersionModel GameVersion { get; set; }

        public Guid Guid { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public long Size { get; set; }
    }
}