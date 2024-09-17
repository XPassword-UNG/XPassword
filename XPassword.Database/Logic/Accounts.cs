using System.Text.RegularExpressions;
using XPassword.Database.Model;
using XPassword.Database.Model.Exceptions;
using XPassword.Security;

namespace XPassword.Database.Logic;

public sealed class Accounts : IDisposable
{
    private readonly ResourceAccess.Accounts _resourceAccess = new();

    public bool CreateAccount(string username, string email, string password, string confirmedPassword)
    {
        var validation = new ValidationException();

        if (username.Trim().Length < 5)
            validation.AddError("Username must have at least 5 characters!");

        if (!IsValidEmail(email))
            validation.AddError("Email must be a valid one!");

        if (password != confirmedPassword)
            validation.AddError("Passwords must match!");

        if (_resourceAccess.CheckIfAccountExists(email))
            validation.AddError("Account already exist");

        if (validation.HasError)
            throw validation;

        return _resourceAccess.CreateAccount(username, email, password);
    }

    public bool LogIn(string email, string password)
    {
        if (!_resourceAccess.CheckIfAccountExists(email))
            return false;

        var account = _resourceAccess.GetAccount(email, password);

        if (account == null)
            return false;

        return Hasher.VerifyPassword(account.HPassword, password);
    }

    public Account? GetAccount(string email, string password) => _resourceAccess.GetAccount(email, password);

    #region [ Util ]
    private static bool IsValidEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            return false;

        var pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        var regex = new Regex(pattern);

        return regex.IsMatch(email);
    }
    #endregion

    #region [ Dispose ]
    private void Dispose(bool disposing)
    {
        if (disposing)
            _resourceAccess?.Dispose();
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion
}