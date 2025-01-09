namespace ROH.Service.Log;

public interface ILogService
{
    Task LogException(string exception);
}
