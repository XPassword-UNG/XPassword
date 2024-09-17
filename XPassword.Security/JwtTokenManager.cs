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

    public static string GenerateJwtToken(string email, string password, int lifetimeInSeconds)
    {
        var encryption_key = EncryptionManager.GenerateRandomKey();
        var claims = new[]
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.NameIdentifier, password.Encrypt(encryption_key)!),
            new Claim(ClaimTypes.UserData, encryption_key),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SupSecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            claims: claims,
            issuer: "MyAuthServer",
            audience: "MyApi",
            expires: DateTime.Now.AddSeconds(lifetimeInSeconds),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static (string email, string password) ExtractEmailAndPassword(ClaimsPrincipal user)
    {
        var email = user.Claims.Where(c => c.Type == ClaimTypes.Email).ToList().First().Value;
        var passrd = user.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).ToList().First().Value;
        var key = user.Claims.Where(c => c.Type == ClaimTypes.UserData).ToList().First().Value;

        return (email, passrd.Decrypt(key)!);
    }
}