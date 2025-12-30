//-----------------------------------------------------------------------
// <copyright file="AuthService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.IdentityModel.Tokens;

using ROH.Service.Authentication.Interface;
using ROH.StandardModels.Account;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ROH.Service.Authentication;

public class AuthService : IAuthService
{
    public string GenerateJwtToken(UserModel user)
    {
        string keyToken = Environment.GetEnvironmentVariable("ROH_KEY_TOKEN") ?? "thisisaverysecurekeywith32charslong!";
        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = Encoding.ASCII.GetBytes(keyToken);
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject =
                new ClaimsIdentity(
                    new Claim[]
                {
                    new(JwtRegisteredClaimNames.Sub, user.UserName!),
                    new(JwtRegisteredClaimNames.Jti, user.Guid!.Value.ToString())
                }),
            Expires = DateTime.UtcNow.AddHours(24),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = "ROH.Gateway",
            Issuer = "ROH.Services.Authentication.AuthService"
        };
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}