using XPassword.Database.Data;
using XPassword.Database.Model;
using XPassword.Database.ResourceAccess.Interfaces;
using XPassword.Security;

namespace XPassword.Database.ResourceAccess;

internal sealed class Accounts : IAccounts
{
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
        command.Parameters.AddWithValue("@hpass", Hasher.HashPassword(password));

        command.CommandText = sql;

        return command.ExecuteNonQuery() == 1;
    }

    public bool CheckIfAccountExists(string email)
    {
        using var command = _database.CreateCommand();

        var sql = @$"SELECT 
                        Email
                    FROM
                        {DatabaseContext.ACC_TABLE}
                    WHERE
                        Email = @email";

        command.Parameters.AddWithValue("@email", email);
        command.CommandText = sql;

        var reader = command.ExecuteReader();
        
        return Builder.BuildEmailExist(reader);
    }

    public bool UpdateAccount(string email, string password, string newUsername, string newEmail)
    {
        using var command = _database.CreateCommand();

        var sql = @$"UPDATE 
                        {DatabaseContext.ACC_TABLE}
                    SET
                        Name = @newUsername,
                        Email = @newEmail
                    WHERE
                        Email = @email AND
                        HPassword = @hPassword";

        command.Parameters.AddWithValue("@newUsername", email);
        command.Parameters.AddWithValue("@newEmail", email);
        command.Parameters.AddWithValue("@email", email);
        command.Parameters.AddWithValue("@hPassword", password);
        command.CommandText = sql;

        return command.ExecuteNonQuery() == 1;
    }

    public Account? GetAccount(string email, string password)
    {
        using var command = _database.CreateCommand();

        var sql = @$"SELECT 
                        Id,
                        Name,
                        Email,
                        HPassword
                    FROM
                        {DatabaseContext.ACC_TABLE}
                    WHERE
                        Email = @email";

        command.Parameters.AddWithValue("@email", email);
        command.CommandText = sql;

        var reader = command.ExecuteReader();
        var accList = Builder.BuildAccounts(reader);

        if (accList != null && accList.Count == 1)
            return accList[0];

        return null;
    }

    public int DeleteAccount(string email, string password)
    {
        using var command = _database.CreateCommand();

        var sql = @$"DELETE FROM
                        {DatabaseContext.ACC_TABLE}
                    WHERE
                        Email = @email AND
                        HPassword = @pass";

        command.Parameters.AddWithValue("@email", email);
        command.Parameters.AddWithValue("@pass", password);
        command.CommandText = sql;

        return command.ExecuteNonQuery();
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