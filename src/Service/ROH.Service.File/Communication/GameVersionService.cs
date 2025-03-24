using Grpc.Net.Client;

using ROH.Service.File.Interface;
using ROH.Utils.ApiConfiguration;

using static ROH.Utils.ApiConfiguration.ApiConfigReader;

namespace ROH.Service.File.Communication;

public class GameVersionService : IGameVersionService
{
    private static readonly ApiConfigReader _apiConfig = new();
    private static readonly Dictionary<ApiUrl, Uri> _apiUrl = _apiConfig.GetApiUrl();

    public async Task<VersionServiceApi.DefaultResponse> GetCurrentVersionAsync(CancellationToken cancellationToken = default)
    {
#pragma warning disable S4830 // Server certificates should be verified during SSL/TLS connections
        var channel = GrpcChannel.ForAddress(
            _apiUrl.GetValueOrDefault(ApiUrl.VersionGrpc) ?? new Uri(string.Empty),
            new GrpcChannelOptions
            {
                HttpHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                }
            });
#pragma warning restore S4830 // Server certificates should be verified during SSL/TLS connections
        var client = new VersionServiceApi.GameVersionService.GameVersionServiceClient(channel);

        return await client.GetCurrentVersionAsync(new VersionServiceApi.Empty(), cancellationToken: cancellationToken).ConfigureAwait(true);
    }

    public async Task<bool> VerifyIfVersionExistAsync(System.Guid versionGuid, CancellationToken cancellationToken = default)
    {
#pragma warning disable S4830 // Server certificates should be verified during SSL/TLS connections -- need to be fixed in production
        var channel = GrpcChannel.ForAddress(_apiUrl.GetValueOrDefault(ApiUrl.VersionGrpc) ?? new Uri(string.Empty),
            new GrpcChannelOptions
            {
                HttpHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                }
            });

#pragma warning restore S4830 // Server certificates should be verified during SSL/TLS connections
        var client = new VersionServiceApi.GameVersionService.GameVersionServiceClient(channel);

        var response = await client.VerifyIfVersionExistAsync(new VersionServiceApi.Guid() { Guid_ = versionGuid.ToString() }, cancellationToken: cancellationToken).ConfigureAwait(true);

        return response.Result;
    }
}
