namespace ROH.Gateway.Realtime;

public interface IRealtimeIdentityService
{
    Task<AuthenticatedRealtimeIdentity> AuthenticateAsync(
        HttpContext context,
        CancellationToken cancellationToken = default);
}
