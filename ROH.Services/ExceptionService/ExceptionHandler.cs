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
            // Log the exception (e.g., to a file or logging service)
            LogException(exception);

            // Send an email with the exception details
            SendEmail(exception);

            // Return a friendly message to the user
            return new DefaultResponse(httpStatus: System.Net.HttpStatusCode.BadRequest, message: "An error has occurred. Don't be afraid; an email with the error details has been sent to your developers.");
        }

        private void LogException(Exception exception) => _logRepository.SaveLog(new Domain.Logging.Log(0, Domain.Logging.Severity.Error, $@"Source: {exception.Source};Message: {exception.Message}; StackTrace: {exception.StackTrace}"));

        private void SendEmail(Exception exception)
        {
            // Implement your email sending logic here
            // For example, use an email sending library or service
            // Send an email to this.emailRecipient with the exception details
        }
    }

}
