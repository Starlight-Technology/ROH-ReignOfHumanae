//-----------------------------------------------------------------------
// <copyright file="LogService.cs" company="Starlight-Technology">
//     Author:
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Grpc.Net.Client;

using LogServiceApi;

using ROH.Service.Exception.Interface;
using ROH.Utils.ApiConfiguration;

using static ROH.Utils.ApiConfiguration.ApiConfigReader;

namespace ROH.Service.Exception.Communication;

public class LogService : ILogService
{
    static readonly ApiConfigReader _apiConfig = new();
    static readonly Dictionary<ApiUrl, Uri> _apiUrl = _apiConfig.GetApiUrl();

    public async Task SaveLog(string message)
    {
#pragma warning disable S4830 // Server certificates should be verified during SSL/TLS connections
        GrpcChannel channel = GrpcChannel.ForAddress(
            _apiUrl.GetValueOrDefault(ApiUrl.LogGrpc) ?? new Uri(string.Empty),
            new GrpcChannelOptions
            {
                HttpHandler =
                    new HttpClientHandler
                        {
                            ServerCertificateCustomValidationCallback =
                                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                        }
            });
#pragma warning restore S4830 // Server certificates should be verified during SSL/TLS connections
        LogServiceApi.LogService.LogServiceClient client = new LogServiceApi.LogService.LogServiceClient(channel);

        await client.LogAsync(new LogRequest { Message = message });
    }
}