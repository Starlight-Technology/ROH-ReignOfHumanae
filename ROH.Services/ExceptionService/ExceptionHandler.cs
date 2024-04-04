using ROH.Domain.Logging;
using ROH.Interfaces.Repository.Log;
using ROH.Interfaces.Services.ExceptionService;
using ROH.StandardModels.Response;

namespace ROH.Services.ExceptionService
{
    public class ExceptionHandler(ILogRepository logRepository) : IExceptionHandler
    {
        private readonly ILogRepository _logRepository = logRepository;

        public DefaultResponse HandleException(Exception exception)
        {
            string error = $@"Source: {exception.Source};Message: {exception.Message}; StackTrace: {exception.StackTrace}";

            // Log the exception (e.g., to a file or logging service)
            LogException(error);

            Console.WriteLine(error);

            // Return a friendly message to the user unless are in DEBUG
#if DEBUG
            return new DefaultResponse(httpStatus: System.Net.HttpStatusCode.BadRequest, message: error);
#else
            return new DefaultResponse(httpStatus: System.Net.HttpStatusCode.BadRequest, message: "An error has occurred. Don't be afraid; an email with the error details has been sent to your developers.");
#endif
        }

        private void LogException(string exception) => _logRepository.SaveLog(new Log(0, Domain.Logging.Severity.Error, exception));
    }
}
