using ROH.Interfaces;
using ROH.Interfaces.Repository.Log;

namespace ROH.Repository.Log;

public class LogRepository(ISqlContext context) : ILogRepository
{
    public async Task SaveLog(Domain.Logging.Log log)
    {
        _ = await context.Logs.AddAsync(log);
        _ = await context.SaveChangesAsync();
    }
}
