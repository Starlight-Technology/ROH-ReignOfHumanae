using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ROH.Services.Authentication;
using ROH.StandardModels.Account;
using Xunit;

namespace ROH.Services.Tests.Authentication
{
    public class AuthServiceTest
    {
        [Fact]
        public void GenerateJwtToken_ShouldReturnValidToken()
        {
            // Arrange
            var authService = new AuthService();
            var user = new UserModel
            {
                UserName = "TestUser",
                Guid = Guid.NewGuid()
            };

            // Act
            var token = authService.GenerateJwtToken(user);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(token));

            // Decode and validate token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("ROH_KEY_TOKEN") ?? "thisisaverysecurekeywith32charslong!");

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = "ROH.Services.Authentication.AuthService",
                ValidateAudience = true,
                ValidAudience = "ROH.Gateway",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero // No clock skew
            }, out SecurityToken validatedToken);

            // Check claims
            var jwtToken = (JwtSecurityToken)validatedToken;
            var usernameClaim = jwtToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;
            var jtiClaim = jwtToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Jti).Value;

            Assert.Equal(user.UserName, usernameClaim);
            Assert.Equal(user.Guid.Value.ToString(), jtiClaim);
        }
    }
}
