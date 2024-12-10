
using ROH.Context.Log.Enums;
using ROH.Context.Log.Interface;

namespace ROH.Service.Log;

public class LogService(ILogRepository logRepository)
: ILogService
{
    public Task LogException(string exception) =>
        logRepository.SaveLogAsync(new Context.Log.Entities.Log(0, Severity.Error, exception), CancellationToken.None);
}
