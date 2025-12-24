using MessagePack;

using Microsoft.IdentityModel.Tokens;

using ROH.Contracts.GRPC.Player.NearbyPlayer;
using ROH.Contracts.GRPC.Player.PlayerPosition;
using ROH.Contracts.WebSocket.Player;
using ROH.Gateway.Grpc.Player;
using ROH.Service.Exception.Interface;

using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;

namespace ROH.Gateway.WebSocketGateway;

public class RealtimeConnectionManager
{
    private readonly ConcurrentDictionary<string, WebSocket> _clients = new();

    private readonly PlayerSavePositionServiceForwarder _savePositionForwarder;
    private readonly NearbyPlayerServiceForwarder _nearbyForwarder;
    private readonly IExceptionHandler exceptionHandler;
    string accountGuid;

    public RealtimeConnectionManager(
        PlayerSavePositionServiceForwarder savePositionForwarder,
        NearbyPlayerServiceForwarder nearbyForwarder)
    {
        _savePositionForwarder = savePositionForwarder;
        _nearbyForwarder = nearbyForwarder;
    }

    public async Task HandleClientAsync(HttpContext ctx, WebSocket socket)
    {

        try
        {
            accountGuid = Authenticate(ctx);
        }
        catch
        {
            await socket.CloseAsync(
                WebSocketCloseStatus.PolicyViolation,
                "Unauthorized",
                CancellationToken.None);

            return;
        }

        _clients[accountGuid] = socket;

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
        }
        catch (Exception e)
        {
            exceptionHandler.HandleException(e);
        }
        finally
        {
            _clients.TryRemove(accountGuid, out _);

            if (socket.State == WebSocketState.Open)
            {
                await socket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "Disconnected",
                    CancellationToken.None);
            }
        }
    }

    private async Task HandleMessage(RealtimeEnvelope env, WebSocket socket)
    {
        switch (env.Type)
        {
            case "PlayerPosition":
                await HandlePlayerPosition(env.Payload, socket);
                break;
        }
    }

    private async Task HandlePlayerPosition(byte[] payload, WebSocket socket)
    {
        var msg = MessagePackSerializer.Deserialize<PlayerPositionMessage>(payload);

        var clientKey = msg.PlayerId;
        _clients[clientKey] = socket;
        // ------------------------------------------------------------------
        // 1️⃣ Salva posição (PlayerSync.SavePosition)
        // ------------------------------------------------------------------
        await _savePositionForwarder.SavePlayerData(
            new PlayerRequest
            {
                PlayerId = msg.PlayerId,
                Position = new Position
                {
                    X = msg.X,
                    Y = msg.Y,
                    Z = msg.Z
                },
                Rotation = new Rotation
                {
                    X = msg.RotX,
                    Y = msg.RotY,
                    Z = msg.RotZ,
                    W = msg.RotW
                }

            },

            context: null! // Gateway não usa metadata aqui
        );

        // ------------------------------------------------------------------
        // 2️⃣ Busca jogadores próximos
        // ------------------------------------------------------------------
        var nearby = await _nearbyForwarder.GetNearbyPlayers(
            new NearbyPlayersRequest
            {
                PlayerId = msg.PlayerId,
                X = msg.X,
                Y = msg.Y,
                Z = msg.Z,
                Radius = msg.Radius
            },
            context: null!
        );

        // ------------------------------------------------------------------
        // 3️⃣ Envia resposta apenas para o jogador solicitante
        // ------------------------------------------------------------------
        if (_clients.TryGetValue(msg.PlayerId, out socket))
        {
            var response = new NearbyPlayersMessage
            {
                Players = [.. nearby.Players.Select(p => new NearbyPlayerMessage
                {
                    PlayerId = p.PlayerId,
                    X = p.X,
                    Y = p.Y,
                    Z = p.Z,
                    RotX = p.RotX,
                    RotY = p.RotY,
                    RotZ = p.RotZ,
                    RotW = p.RotW,
                    ModelName = p.ModelName,
                    AnimationState = p.AnimationState
                })]
            };

            await SendAsync(
                socket,
                new RealtimeEnvelope
                {
                    Type = "NearbyPlayers",
                    Payload = MessagePackSerializer.Serialize(response)
                });
        }
    }

    private static async Task SendAsync(WebSocket socket, RealtimeEnvelope env)
    {
        var data = MessagePackSerializer.Serialize(env);

        await socket.SendAsync(
            data,
            WebSocketMessageType.Binary,
            true,
            CancellationToken.None);
    }

    private string Authenticate(HttpContext ctx)
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

        // 🔑 playerId vem do JTI
        string? accountGuid = principal.Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)
            ?.Value;

        if (string.IsNullOrWhiteSpace(accountGuid))
            throw new UnauthorizedAccessException("JWT sem PlayerId.");

        return accountGuid;
    }
}
