using Microsoft.Extensions.Options;

using ROH.Context.Player.Mongo.Interface;

namespace ROH.Gateway.Realtime;

public class ChatCleanupService(
    IChatRepository repository,
    IOptions<RealtimeOptions> options,
    ILogger<ChatCleanupService> logger) : BackgroundService
{
    private readonly RealtimeOptions _options = options.Value;

    public static DateTime CalculateCutoffUtc(DateTime nowUtc, int retentionHours)
    {
        int safeRetentionHours = Math.Max(1, retentionHours);
        return nowUtc.Subtract(TimeSpan.FromHours(safeRetentionHours));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new(TimeSpan.FromHours(_options.ChatCleanupIntervalHours));

        await CleanupAsync(stoppingToken).ConfigureAwait(false);

        while (await timer.WaitForNextTickAsync(stoppingToken).ConfigureAwait(false))
            await CleanupAsync(stoppingToken).ConfigureAwait(false);
    }

    private async Task CleanupAsync(CancellationToken cancellationToken)
    {
        try
        {
            // Multiple Gateway instances may run this idempotent delete concurrently.
            DateTime cutoffUtc = CalculateCutoffUtc(DateTime.UtcNow, _options.ChatRetentionHours);
            long deletedCount = await repository
                .DeleteOlderThanAsync(cutoffUtc, cancellationToken)
                .ConfigureAwait(false);

            logger.LogInformation(
                "Chat cleanup removed {DeletedCount} messages older than {CutoffUtc}.",
                deletedCount,
                cutoffUtc);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Chat cleanup failed.");
        }
    }
}
