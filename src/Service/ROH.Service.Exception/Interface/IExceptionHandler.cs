//-----------------------------------------------------------------------
// <copyright file="IExceptionHandler.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.StandardModels.Response;

namespace ROH.Service.Exception.Interface;

public interface IExceptionHandler
{
    DefaultResponse HandleException(System.Exception exception);
}