using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace XPassword.Security;

public static class JwtTokenManager
{
    private static string? _superSecretKey = null;

    public static string SupSecretKey => _superSecretKey ??= GenerateSecretKey();

    private static string GenerateSecretKey()
    {
        var key = new byte[32]; // 256 bits
        RandomNumberGenerator.Fill(key);
        return Convert.ToBase64String(key);
    }

    public static string GenerateJwtToken(string usermail, int lifetimeInSeconds)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usermail),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SupSecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            //issuer: "YourIssuer",
            //audience: "YourAudience",
            claims: claims,
            expires: DateTime.Now.AddSeconds(lifetimeInSeconds),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}