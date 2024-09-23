using XPassword.Database.Model;

namespace XPassword.Database.Logic;

public sealed class Registers : IDisposable
{
    private readonly ResourceAccess.Accounts _resourceAccountsAccess = new();
    private readonly ResourceAccess.Registers _resourceRegistersAccess = new();

    public int AddRegisters(List<Register> registers, string email, string password)
    {
        var account = _resourceAccountsAccess.GetAccount(email, password);

        if (account == null)
            return 0;

        return _resourceRegistersAccess.InsertRegisters(registers.Encrypt(password), account.Id);
    }

    public List<Register> GetRegisters(string email, string password)
    {
        var account = _resourceAccountsAccess.GetAccount(email, password);

        if (account == null)
            return [];

        return _resourceRegistersAccess.GetRegisters(account.Id).Decrypt(password);
    }

    public bool UpdateRegister(string email, string password, Register register)
    {
        var account = _resourceAccountsAccess.GetAccount(email, password);

        if (account == null)
            return false;

        return _resourceRegistersAccess.UpdateRegister(register.Encrypt(password));
    }

    public bool DeleteRegister(string email, string password, Register register)
    {
        var account = _resourceAccountsAccess.GetAccount(email, password);

        if (account == null)
            return false;

        return _resourceRegistersAccess.DeleteRegister(register.Encrypt(password));
    }

    #region [ Dispose ]
    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            _resourceAccountsAccess?.Dispose();
            _resourceRegistersAccess?.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion
}