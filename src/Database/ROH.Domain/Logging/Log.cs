//-----------------------------------------------------------------------
// <copyright file="Log.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace ROH.Domain.Logging;

public record Log(long Id, Severity Severity, string Message)
{
    public DateTime Date { get; } = DateTime.Now;
}
