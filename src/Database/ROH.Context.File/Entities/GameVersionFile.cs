//-----------------------------------------------------------------------
// <copyright file="GameVersionFile.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

//-----------------------------------------------------------------------
// <copyright file="GameVersionFile.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ROH.Context.File.Entities;

public record GameVersionFile(long Id = 0, Guid GuidVersion = default, long IdGameFile = 0, Guid Guid = default)
{
    public virtual GameFile? GameFile { get; set; }
}