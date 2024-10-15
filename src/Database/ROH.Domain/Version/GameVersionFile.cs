//-----------------------------------------------------------------------
// <copyright file="GameVersionFile.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.Domain.GameFiles;

namespace ROH.Domain.Version;

public record GameVersionFile(long Id = 0, long IdVersion = 0, long IdGameFile = 0, Guid Guid = default)
{
    public virtual GameFile? GameFile { get; set; }

    public virtual GameVersion? GameVersion { get; set; }
}