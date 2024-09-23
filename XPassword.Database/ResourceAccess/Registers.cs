using System.Data.SQLite;
using XPassword.Database.Data;
using XPassword.Database.Model;
using XPassword.Database.ResourceAccess.Interfaces;

namespace XPassword.Database.ResourceAccess;

internal sealed class Registers : IRegisters
{
    private readonly DatabaseContext _database = new();

    public int InsertRegisters(List<Register> registers, long userId)
    {
        using var command = _database.CreateCommand();

        var sql = @$"INSERT INTO {DatabaseContext.REG_TABLE} (UserId, Name, Email, Description, Password) VALUES ";
        var parameters = new List<SQLiteParameter>();

        for (int i = 0; i < registers.Count; i++)
        {
            sql += $"(@UserId{i}, @Name{i}, @Email{i}, @Description{i}, @Password{i}),";
            parameters.AddRange(
            [
                new SQLiteParameter($"@UserId{i}", userId),
                new SQLiteParameter($"@Name{i}", registers[i].Name),
                new SQLiteParameter($"@Email{i}", registers[i].Email),
                new SQLiteParameter($"@Description{i}", registers[i].Description),
                new SQLiteParameter($"@Password{i}", registers[i].Password)
            ]);
        }
        sql = sql.TrimEnd(',');
        command.CommandText = sql.ToString();
        command.Parameters.AddRange([.. parameters]);

        return command.ExecuteNonQuery();
    }

    public List<Register> GetRegisters(long userId)
    {
        using var command = _database.CreateCommand();

        var sql = @$"SELECT 
                        Id,
                        UserId,
                        Name,
                        Email,
                        Description,
                        Password
                    FROM
                        {DatabaseContext.REG_TABLE}
                    WHERE
                        UserId = @userId";

        command.Parameters.AddWithValue("@userId", userId);
        command.CommandText = sql;

        var reader = command.ExecuteReader();
        return Builder.BuildRegister(reader);
    }

    public bool UpdateRegister(Register register)
    {
        using var command = _database.CreateCommand();

        var sql = @$"UPDATE
                        {DatabaseContext.REG_TABLE}
                    SET
                        Name = @Name,
                        Email = @Email,
                        Description = @Description,
                        Password = @Password
                    WHERE
                        Id = @Id AND
                        UserId = @userId";

        command.Parameters.AddWithValue("@Id", register.Id);
        command.Parameters.AddWithValue("@userId", register.UserId);
        command.Parameters.AddWithValue("@Name", register.Name);
        command.Parameters.AddWithValue("@Email", register.Email);
        command.Parameters.AddWithValue("@Description", register.Description);
        command.Parameters.AddWithValue("@Password", register.Password);

        command.CommandText = sql;

        var res = command.ExecuteNonQuery();
        return res == 1;
    }

    public bool DeleteRegister(Register register)
    {
        using var command = _database.CreateCommand();

        var sql = @$"DELETE FROM
                        {DatabaseContext.REG_TABLE}
                    WHERE
                        Id = @Id AND
                        UserId = @userId";

        command.Parameters.AddWithValue("@Id", register.Id);
        command.Parameters.AddWithValue("@userId", register.UserId);
        command.CommandText = sql;

        var res = command.ExecuteNonQuery();
        return res == 1;
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