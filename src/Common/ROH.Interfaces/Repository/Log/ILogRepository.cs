namespace ROH.Interfaces.Repository.Log;
public interface ILogRepository
{
    Task SaveLog(Domain.Logging.Log log);
}