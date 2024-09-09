using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using XPassword.Database.Data;
using XPassword.Database.Model;
using XPassword.Database.Model.Exceptions;
using XPassword.Database.ResourceAccess;
using XPassword.Database.ResourceAccess.Interfaces;

namespace XPassword.Database.Logic;

public sealed class Accounts(int? userId = null) : IDisposable
{
    private readonly int? _userId = userId;
    private readonly ResourceAccess.Accounts _resourceAccess = new(userId);

    public bool CreateAccount(string username, string email, string password, string confirmedPassword)
    {
        var validationException = new ValidationException();

        if (username.Trim().Length < 5)
            validationException.AddError("Username must have at least 5 characters!");

        if (!IsValidEmail(email))
            validationException.AddError("Email must be a valid one!");

        if (password != confirmedPassword)
            validationException.AddError("Passwords must match!");

        if (validationException.HasError)
            throw validationException;

        return _resourceAccess.CreateAccount(username, email, password);
    }

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