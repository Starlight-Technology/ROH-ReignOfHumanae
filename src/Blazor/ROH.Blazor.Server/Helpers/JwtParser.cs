//-----------------------------------------------------------------------
// <copyright file="JwtParser.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Security.Claims;
using System.Text.Json;

namespace ROH.Blazor.Server.Helpers;

public static class JwtParser
{
    static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2:
                base64 += "==";
                break;

            case 3:
                base64 += "=";
                break;
        }
        return Convert.FromBase64String(base64);
    }

    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        List<Claim> claims = [];
        string payload = jwt.Split('.')[1];
        byte[] jsonBytes = ParseBase64WithoutPadding(payload);
        Dictionary<string, object>? keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        claims.AddRange(
            from kvp in keyValuePairs
                select new Claim(kvp.Key, kvp.Value?.ToString() ?? string.Empty));
        return claims;
    }
}