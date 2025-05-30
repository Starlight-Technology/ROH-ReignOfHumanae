//-----------------------------------------------------------------------
// <copyright file="ILogService.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ROH.Service.Log;

public interface ILogService
{
    Task LogException(string exception);
}
