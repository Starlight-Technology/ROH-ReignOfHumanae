//-----------------------------------------------------------------------
// <copyright file="GameFile.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace ROH.Domain.GameFiles;

public record GameFile
(
    long Id = 0,
    Guid Guid = default,
    long Size = 0,
    string Name = "",
    string Path = "",
    string Format = "",
    bool Active = true
);
