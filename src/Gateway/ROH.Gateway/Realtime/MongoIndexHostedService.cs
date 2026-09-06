using ROH.Context.Player.Mongo.Interface;

namespace ROH.Gateway.Realtime;

public class MongoIndexHostedService(
    IMongoIndexInitializer indexInitializer,
    ILogger<MongoIndexHostedService> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            await indexInitializer.EnsureIndexesAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "MongoDB indexes could not be initialized.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
