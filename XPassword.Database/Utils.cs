using XPassword.Database.Model;
using XPassword.Security;

namespace XPassword.Database;

internal static class Utils
{
    internal static Register Encrypt(this Register reg, string passwordKey) => new List<Register>(collection: [reg!]).Encrypt(passwordKey).FirstOrDefault()!;

    internal static List<Register> Encrypt(this List<Register> regs, string passwordKey)
    {
        var encryptedRegisters = new List<Register>();

        foreach (var reg in regs)
        {
            encryptedRegisters.Add(new Register()
            {
                Id = reg.Id,
                UserId = reg.UserId,
                Name = reg.Name.Encrypt(passwordKey),
                Description = reg.Description.Encrypt(passwordKey),
                Email = reg.Email.Encrypt(passwordKey),
                Password = reg.Password.Encrypt(passwordKey)
            });
        }

        return encryptedRegisters;
    }

    internal static List<Register> Decrypt(this List<Register> regs, string passwordKey)
    {
        var encryptedRegisters = new List<Register>();

        foreach (var reg in regs)
        {
            encryptedRegisters.Add(new Register()
            {
                Id = reg.Id,
                UserId = reg.UserId,
                Name = reg.Name.Decrypt(passwordKey),
                Description = reg.Description.Decrypt(passwordKey),
                Email = reg.Email.Decrypt(passwordKey),
                Password = reg.Password.Decrypt(passwordKey)
            });
        }

        return encryptedRegisters;
    }
}