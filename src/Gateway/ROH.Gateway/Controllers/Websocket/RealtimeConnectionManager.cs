using MessagePack;

using Microsoft.IdentityModel.Tokens;

using ROH.Contracts.WebSocket;
using ROH.Service.Exception.Interface;
using ROH.Service.Player.WebSocket.Interface;

using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;

namespace ROH.Service.WebSocket;

public class RealtimeConnectionManager(
    IExceptionHandler exceptionHandler,
    IPlayerPositionServiceSocket playerPositionService)
: IRealtimeConnectionManager
{
    private readonly ConcurrentDictionary<string, System.Net.WebSockets.WebSocket> _accountConnections = new();

    private Contracts.GRPC.Player.PlayerPosition.PlayerService.PlayerServiceClient _savePlayerPositionApi;

    private string accountGuid = string.Empty;

    public async Task HandleClientAsync(HttpContext ctx, System.Net.WebSockets.WebSocket socket)
    {
        try
        {
            accountGuid = Authenticate(ctx);
        }
        catch (System.Exception e)
        {
            await socket.CloseAsync(
                WebSocketCloseStatus.PolicyViolation,
                "Unauthorized",
                CancellationToken.None);

            exceptionHandler.HandleException(e);

            return;
        }

        _accountConnections[accountGuid] = socket;

        var buffer = new byte[8192];

        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer, CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                    break;

                var data = buffer.AsMemory(0, result.Count);

                var options = MessagePackSerializerOptions.Standard;
                var envelope = MessagePackSerializer.Deserialize<RealtimeEnvelope>(data, options);

                await HandleMessage(envelope, socket);
            }
        }
        catch (WebSocketException e)
        {
            Console.WriteLine(e.WebSocketErrorCode);
            exceptionHandler.HandleException(e);
        }
        catch (System.Exception e)
        {
            exceptionHandler.HandleException(e);
        }
        finally
        {
            _accountConnections.TryRemove(accountGuid, out _);

            if (socket.State == WebSocketState.Open)
            {
                await socket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "Disconnected",
                    CancellationToken.None);
            }
        }
    }

    private async Task HandleMessage(RealtimeEnvelope env, System.Net.WebSockets.WebSocket socket)
    {
        switch (env.Type)
        {
            case RealtimeEventTypes.SavePlayerPosition:
                await playerPositionService.HandlePlayerPosition(env.Payload, socket);
                break;
        }
    }

    private static string Authenticate(HttpContext ctx)
    {
        if (!ctx.Request.Query.TryGetValue("access_token", out var tokenValue))
            throw new UnauthorizedAccessException("WebSocket sem token JWT.");

        string token = tokenValue!;

        string keyToken =
            Environment.GetEnvironmentVariable("ROH_KEY_TOKEN")
            ?? "thisisaverysecurekeywith32charslong!";

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(keyToken);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),

            ValidateIssuer = true,
            ValidIssuer = "ROH.Services.Authentication.AuthService",

            ValidateAudience = true,
            ValidAudience = "ROH.Gateway",

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30)
        };

        ClaimsPrincipal principal;

        try
        {
            principal = tokenHandler.ValidateToken(
                token,
                validationParameters,
                out _);
        }
        catch
        {
            throw new UnauthorizedAccessException("JWT inválido.");
        }

        string? accountGuid = principal.Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)
            ?.Value;

        if (string.IsNullOrWhiteSpace(accountGuid))
            throw new UnauthorizedAccessException("JWT sem PlayerId.");

        return accountGuid;
    }
}