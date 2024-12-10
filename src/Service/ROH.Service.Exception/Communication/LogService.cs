using Grpc.Net.Client;

using LogServiceApi;

using ROH.Service.Exception.Interface;

namespace ROH.Service.Exception.Communication;
public class LogService : ILogService
{
    public async Task SaveLog(string message)
    {
        var channel = GrpcChannel.ForAddress("https://localhost:7044");
        var client = new LogServiceApi.LogService.LogServiceClient(channel);

        await client.LogAsync(new LogRequest
        {
            Message = message
        });
    }
}
