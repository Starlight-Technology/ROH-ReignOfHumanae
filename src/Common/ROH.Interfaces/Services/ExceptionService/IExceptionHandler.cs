//-----------------------------------------------------------------------
// <copyright file="IExceptionHandler.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.StandardModels.Response;

namespace ROH.Interfaces.Services.ExceptionService;

public interface IExceptionHandler
{
    DefaultResponse HandleException(Exception exception);
}