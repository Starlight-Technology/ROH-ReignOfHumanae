using Grpc.Net.Client;

using LogServiceApi;

using ROH.Service.Exception.Interface;
using ROH.Utils.ApiConfiguration;

using static ROH.Utils.ApiConfiguration.ApiConfigReader;

namespace ROH.Service.Exception.Communication;

public class LogService : ILogService
{
    private static readonly ApiConfigReader _apiConfig = new();
    private static readonly Dictionary<ApiUrl, Uri> _apiUrl = _apiConfig.GetApiUrl();

    public async Task SaveLog(string message)
    {
#pragma warning disable S4830 // Server certificates should be verified during SSL/TLS connections
        var channel = GrpcChannel.ForAddress(_apiUrl.GetValueOrDefault(ApiUrl.LogGrpc) ?? new Uri(string.Empty),
            new GrpcChannelOptions
            {
                HttpHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                }
            });
#pragma warning restore S4830 // Server certificates should be verified during SSL/TLS connections
        var client = new LogServiceApi.LogService.LogServiceClient(channel);

        await client.LogAsync(new LogRequest
        {
            Message = message
        });
    }
}
