//-----------------------------------------------------------------------
// <copyright file="AuthServiceTest.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.IdentityModel.Tokens;

using ROH.Service.Authentication;
using ROH.StandardModels.Account;

using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ROH.Test.Authentication;

public class AuthServiceTest
{
    [Fact]
    public void GenerateJwtTokenShouldReturnValidToken()
    {
        // Arrange
        AuthService authService = new();
        UserModel user = new() { UserName = "TestUser", Guid = Guid.NewGuid() };

        // Act
        string token = authService.GenerateJwtToken(user);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(token));

        // Decode and validate token
        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = Encoding.ASCII
            .GetBytes(Environment.GetEnvironmentVariable("ROH_KEY_TOKEN") ?? "thisisaverysecurekeywith32charslong!");

        tokenHandler.ValidateToken(
            token,
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = "ROH.Services.Authentication.AuthService",
                ValidateAudience = true,
                ValidAudience = "ROH.Gateway",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero // No clock skew
            },
            out SecurityToken validatedToken);

        // Check claims
        JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;
        string usernameClaim = jwtToken.Claims.First(claim => string.Compare(claim.Type, JwtRegisteredClaimNames.Sub, StringComparison.Ordinal) == 0).Value;
        string jtiClaim = jwtToken.Claims.First(claim => string.Compare(claim.Type, JwtRegisteredClaimNames.Jti, StringComparison.Ordinal) == 0).Value;

        Assert.Equal(user.UserName, usernameClaim);
        Assert.Equal(user.Guid.Value.ToString(), jtiClaim);
    }
}