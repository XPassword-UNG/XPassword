using XPassword.Database.Data;
using XPassword.Database.ResourceAccess.Interfaces;

namespace XPassword.Database.ResourceAccess;

internal sealed class Accounts(int? userId = null) : IAccounts
{
    private readonly int? _userId = userId;
    private readonly DatabaseContext _database = new();

    public bool CreateAccount(string username, string email, string password)
    {
        using var command = _database.CreateCommand();

        var sql = @$"INSERT INTO {DatabaseContext.ACC_TABLE} 
                        (Email, Name, HPassword) 
                    VALUES 
                        (@email, @name, @hpass)";

        command.Parameters.AddWithValue("@email", email);
        command.Parameters.AddWithValue("@name", username);
        command.Parameters.AddWithValue("@hpass", password);

        command.CommandText = sql;

        return command.ExecuteNonQuery() == 1;
    }

    #region [ Dispose ]
    private void Dispose(bool disposing)
    {
        if (disposing)
            _database.Dispose();
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion
}