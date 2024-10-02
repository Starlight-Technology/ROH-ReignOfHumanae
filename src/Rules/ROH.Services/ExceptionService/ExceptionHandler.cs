using Microsoft.Extensions.Configuration;

using ROH.Domain.Logging;
using ROH.Interfaces.Repository.Log;
using ROH.Interfaces.Services.ExceptionService;
using ROH.StandardModels.Response;

namespace ROH.Services.ExceptionService;

public class ExceptionHandler(ILogRepository logRepository, IConfiguration configuration) : IExceptionHandler
{
    private readonly ILogRepository _logRepository = logRepository;
    private readonly bool _isDebugMode = configuration.GetValue<bool>("IsDebugMode");

    public DefaultResponse HandleException(Exception exception)
    {
        string error = $@"Source: {exception.Source};Message: {exception.Message}; StackTrace: {exception.StackTrace}";

        // Log the exception (e.g., to a file or logging service)
        LogException(error);

        // Return a friendly message to the user unless are in DEBUG
        return _isDebugMode
            ? new DefaultResponse(httpStatus: System.Net.HttpStatusCode.InternalServerError, message: error)
            : new DefaultResponse(httpStatus: System.Net.HttpStatusCode.InternalServerError, message: "An error has occurred. Don't be afraid! An email with the error details has been sent to your developers.");
    }

    private void LogException(string exception) => _logRepository.SaveLog(new Log(0, Domain.Logging.Severity.Error, exception));
}
