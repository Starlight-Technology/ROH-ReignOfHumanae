using System.Threading.Tasks;

public interface IRealtimeSocket
{
    Task ConnectAsync(string url);
    Task SendAsync<T>(T message);
    Task DisconnectAsync();
}
