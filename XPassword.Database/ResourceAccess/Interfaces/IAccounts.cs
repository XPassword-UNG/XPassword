namespace XPassword.Database.ResourceAccess.Interfaces;

internal interface IAccounts : IDisposable
{
    bool CreateAccount(string username, string email, string password);
}