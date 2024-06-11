using Microsoft.IdentityModel.Tokens;

using ROH.Interfaces.Authentication;
using ROH.StandardModels.Account;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ROH.Services.Authentication;
public class AuthService : IAuthService
{
    public string GenerateJwtToken(UserModel user)
    {
        string keyToken = Environment.GetEnvironmentVariable("ROH_KEY_TOKEN") ?? "thisisaverysecurekeywith32charslong!";
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(keyToken);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(JwtRegisteredClaimNames.Sub, user.UserName!),
                new(JwtRegisteredClaimNames.Jti, user.Guid!.Value.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(24),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = "ROH.Gateway", 
            Issuer = "ROH.Services.Authentication.AuthService"
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
