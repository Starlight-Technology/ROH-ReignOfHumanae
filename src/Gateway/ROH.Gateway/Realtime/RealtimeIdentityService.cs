using Microsoft.IdentityModel.Tokens;

using ROH.Context.Player.Entities.Characters;
using ROH.Context.Player.Interface;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace ROH.Gateway.Realtime;

public partial class RealtimeIdentityService(IServiceScopeFactory scopeFactory) : IRealtimeIdentityService
{
    public async Task<AuthenticatedRealtimeIdentity> AuthenticateAsync(
        HttpContext context,
        CancellationToken cancellationToken = default)
    {
        ClaimsPrincipal principal = ValidateToken(context);
        string accountId = principal.Claims
            .FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Jti)
            ?.Value ?? throw new UnauthorizedAccessException("JWT sem AccountId.");

        if (!Guid.TryParse(accountId, out Guid accountGuid))
            throw new UnauthorizedAccessException("AccountId inválido.");

        string characterIdValue = context.Request.Query["character_id"].ToString();
        if (!Guid.TryParse(characterIdValue, out Guid characterGuid))
            throw new UnauthorizedAccessException("CharacterId ausente ou inválido.");

        await using AsyncServiceScope scope = scopeFactory.CreateAsyncScope();
        ICharacterRepository repository = scope.ServiceProvider.GetRequiredService<ICharacterRepository>();
        ROH.Context.Player.Interface.IPositionRepository positionRepository = scope.ServiceProvider
            .GetRequiredService<ROH.Context.Player.Interface.IPositionRepository>();
        Character? character = await repository
            .GetCharacterByIdAsync(characterGuid, cancellationToken)
            .ConfigureAwait(false);

        if (character is null || character.GuidAccount != accountGuid)
            throw new UnauthorizedAccessException("O personagem não pertence à conta autenticada.");

        string worldId = SanitizeWorldId(context.Request.Query["world_id"].ToString());
        string displayName = string.IsNullOrWhiteSpace(character.Name) ? character.Guid.ToString() : character.Name;
        PlayerPosition? initialPosition = await positionRepository
            .GetPosition(character.Id, cancellationToken)
            .ConfigureAwait(false);

        RealtimePlayerTransform initialTransform = new(
            initialPosition?.Position?.X ?? 0,
            initialPosition?.Position?.Y ?? 0,
            initialPosition?.Position?.Z ?? 0,
            initialPosition?.Rotation?.X ?? 0,
            initialPosition?.Rotation?.Y ?? 0,
            initialPosition?.Rotation?.Z ?? 0,
            initialPosition?.Rotation?.W ?? 1);

        return new AuthenticatedRealtimeIdentity(
            accountGuid.ToString(),
            character.Guid.ToString(),
            displayName,
            worldId,
            initialTransform);
    }

    static string SanitizeWorldId(string worldId)
    {
        if (string.IsNullOrWhiteSpace(worldId))
            return "default";

        string value = WorldIdRegex().Replace(worldId.Trim(), string.Empty);
        return string.IsNullOrEmpty(value) ? "default" : value[..Math.Min(value.Length, 64)];
    }

    static ClaimsPrincipal ValidateToken(HttpContext context)
    {
        if (!context.Request.Query.TryGetValue("access_token", out var tokenValue))
            throw new UnauthorizedAccessException("WebSocket sem token JWT.");

        string keyToken =
            Environment.GetEnvironmentVariable("ROH_KEY_TOKEN") ?? "thisisaverysecurekeywith32charslong!";

        TokenValidationParameters validationParameters = new()
        {
            ClockSkew = TimeSpan.FromSeconds(30),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(keyToken)),
            ValidAudience = "ROH.Gateway",
            ValidIssuer = "ROH.Services.Authentication.AuthService",
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true
        };

        try
        {
            return new JwtSecurityTokenHandler().ValidateToken(tokenValue!, validationParameters, out _);
        }
        catch (Exception exception) when (exception is SecurityTokenException or ArgumentException)
        {
            throw new UnauthorizedAccessException("JWT inválido.", exception);
        }
    }

    [GeneratedRegex("[^A-Za-z0-9_.-]")]
    private static partial Regex WorldIdRegex();
}
