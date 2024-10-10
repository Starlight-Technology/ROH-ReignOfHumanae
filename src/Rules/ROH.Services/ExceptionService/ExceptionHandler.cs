//-----------------------------------------------------------------------
// <copyright file="ExceptionHandler.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.Extensions.Configuration;

using ROH.Domain.Logging;
using ROH.Interfaces.Repository.Log;
using ROH.Interfaces.Services.ExceptionService;
using ROH.StandardModels.Response;

using System.Net;

namespace ROH.Services.ExceptionService;

public class ExceptionHandler(ILogRepository logRepository, IConfiguration configuration) : IExceptionHandler
{
    private readonly bool _isDebugMode = configuration.GetValue<bool>("IsDebugMode");

    private void LogException(string exception) => _logRepository.SaveLog(new Log(0, Severity.Error, exception));

    public DefaultResponse HandleException(Exception exception)
    {
        string error = $@"Source: {exception.Source};Message: {exception.Message}; StackTrace: {exception.StackTrace}";

        // Log the exception (e.g., to a file or logging service)
        LogException(error);

        // Return a friendly message to the user unless are in DEBUG
        return _isDebugMode
            ? new DefaultResponse(httpStatus: HttpStatusCode.InternalServerError, message: error)
            : new DefaultResponse(
                httpStatus: HttpStatusCode.InternalServerError,
                message: "An error has occurred. Don't be afraid! An email with the error details has been sent to your developers.");
    }

    private readonly ILogRepository _logRepository = logRepository;
}
