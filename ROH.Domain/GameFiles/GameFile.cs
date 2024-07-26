﻿namespace ROH.Domain.GameFiles;
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