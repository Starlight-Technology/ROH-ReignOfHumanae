//-----------------------------------------------------------------------
// <copyright file="ILogService.cs" company="Starlight-Technology">
//     Author:
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ROH.Service.Exception.Interface;

public interface ILogService
{
    Task SaveLog(string message);
}