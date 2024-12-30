//-----------------------------------------------------------------------
// <copyright file="ExceptionHandler.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.Service.Exception.Interface;
using ROH.StandardModels.Response;

using System.Net;

namespace ROH.Service.Exception;

public class ExceptionHandler(ILogService logService) : IExceptionHandler
{
    private readonly bool _isDebugMode =
#if DEBUG
        true;
#else
        false;
#endif

    private void LogException(string exception) => logService.SaveLog(exception).ConfigureAwait(true);

    public DefaultResponse HandleException(System.Exception exception)
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
}
