namespace ROH.Service.Exception.Interface;

public interface ILogService
{
    Task SaveLog(string message);
}
