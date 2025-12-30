//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Grpc.Net.Client;

using ROH.Contracts.GRPC.Player.NearbyPlayer;
using ROH.Contracts.GRPC.Worker.PlayerSocket;
using ROH.Service.Exception;
using ROH.Service.Exception.Communication;
using ROH.Utils.ApiConfiguration;

using static ROH.Utils.ApiConfiguration.ApiConfigReader;

const int DEFAULT_NEARBY_RANGE = 100;

Console.WriteLine("Initialized GetNearbyPlayers worker.");

using CancellationTokenSource cts = new CancellationTokenSource();

Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

ApiConfigReader _apiConfig = new();
Dictionary<ApiUrl, Uri> _apiUrl = _apiConfig.GetApiUrl();

#pragma warning disable S4830 // Server certificates should be verified during SSL/TLS connections
GrpcChannel gatewayChannel = GrpcChannel.ForAddress(
    _apiUrl.GetValueOrDefault(ApiUrl.GateWayGrpc) ?? new Uri(string.Empty),
    new GrpcChannelOptions
    {
        HttpHandler =
            new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                }
    });

GrpcChannel nearbyPlayerChannel = GrpcChannel.ForAddress(
    _apiUrl.GetValueOrDefault(ApiUrl.PlayerState) ?? new Uri(string.Empty),
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

PlayerConnectedService.PlayerConnectedServiceClient playerSocketGrpc = new PlayerConnectedService.PlayerConnectedServiceClient(
    gatewayChannel);
NearbyPlayerService.NearbyPlayerServiceClient nearbyPlayerApi = new NearbyPlayerService.NearbyPlayerServiceClient(
    nearbyPlayerChannel);

LogService logService = new LogService();
ExceptionHandler exceptionHandler = new ExceptionHandler(logService);

while (!cts.Token.IsCancellationRequested)
{
    try
    {
        PlayersConnected connectedPlayers =
            await playerSocketGrpc.GetConnectedPlayersAsync(new Default());

        IEnumerable<Task> tasks = connectedPlayers.PlayersId
            .Select(
                async player =>
                {
                    NearbyPlayersResponse nearbyPlayers =
                await nearbyPlayerApi.GetNearbyPlayersAsync(
                        new NearbyPlayersRequest
                            {
                                PlayerId = player.Id,
                                Radius = Math.Max(player.NearbyRadius, DEFAULT_NEARBY_RANGE)
                            });

                    await playerSocketGrpc.SendNearbyPlayersAsync(nearbyPlayers);
                });

        await Task.WhenAll(tasks);
    }
    catch (Exception e)
    {
        exceptionHandler.HandleException(e);
        Console.WriteLine(e.ToString());
        await Task.Delay(10000);
    }

    await Task.Delay(50);
}