//-----------------------------------------------------------------------
// <copyright file="Log.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using ROH.Context.Log.Enums;

namespace ROH.Context.Log.Entities;

public record Log(long Id, Severity Severity, string Message)
{
    public DateTime Date { get; } = DateTime.UtcNow;
}