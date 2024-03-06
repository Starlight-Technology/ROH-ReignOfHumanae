using ROH.StandardModels.Response;

namespace ROH.Interfaces.Services.ExceptionService;
public interface IExceptionHandler
{
    DefaultResponse HandleException(Exception exception);
}