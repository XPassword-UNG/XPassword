using Isopoh.Cryptography.Argon2;
using System.Security.Cryptography;
using System.Text;

namespace XPassword.Security;

public static class Hasher
{
    public static string HashPassword(string password)
    {
        byte[] salt = new byte[16];

        RandomNumberGenerator.Fill(salt);

        var config = new Argon2Config
        {
            Type = Argon2Type.DataIndependentAddressing, // Argon2i
            Version = Argon2Version.Nineteen,
            TimeCost = 4,
            MemoryCost = 1 << 16, // 64MB
            Lanes = 4,
            Threads = Environment.ProcessorCount,
            Salt = salt,
            HashLength = 32,
            Password = Encoding.UTF8.GetBytes(password)
        };

        return Argon2.Hash(config);
    }

    public static bool VerifyPassword(string hash, string password) => Argon2.Verify(hash, password);
}