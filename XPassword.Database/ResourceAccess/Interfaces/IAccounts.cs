using XPassword.Database.Model;

namespace XPassword.Database.ResourceAccess.Interfaces;

internal interface IAccounts : IDisposable
{
    bool CreateAccount(string username, string email, string password);
    bool CheckIfAccountExists(string email);
    Account? GetAccount(string email, string password);
}