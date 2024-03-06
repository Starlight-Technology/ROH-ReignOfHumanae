using ROH.Interfaces;
using ROH.Interfaces.Repository.Log;

namespace ROH.Repository.Log;
public class LogRepository(ISqlContext context) : ILogRepository
{
    public async Task SaveLog(Domain.Logging.Log log)
    {
        await context.Logs.AddAsync(log);
        await context.SaveChangesAsync();
    }
}
