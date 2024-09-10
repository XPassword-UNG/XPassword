using System.Data.SQLite;

namespace XPassword.Database.Data;

internal sealed class DatabaseContext : IDisposable
{
    private readonly SQLiteConnection _connection;

    internal static readonly string ACC_TABLE = "XPASS_ACCOUNT";
    internal static readonly string REG_TABLE = "XPASS_REGISTR";

    internal DatabaseContext()
    {
        var path = Path.Combine(Environment.CurrentDirectory, "XPassword.sqlite");
        _connection = new SQLiteConnection($"Data Source={path}");

        if (!File.Exists(path))
            File.Create(path).Close();

        if (_connection.State != System.Data.ConnectionState.Open)
            _connection.OpenAsync().Wait();

        CreateBaseTable();
    }

    internal SQLiteCommand CreateCommand() => _connection.CreateCommand();

    private void CreateBaseTable()
    {
        using var command = CreateCommand();

        var accountTableCmd = $@"CREATE TABLE IF NOT EXISTS {ACC_TABLE} 
                                (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                                    Email TEXT NOT NULL,
                                    Name TEXT NOT NULL,
                                    HPassword TEXT NOT NULL
                                )";

        command.CommandText = accountTableCmd;
        command.ExecuteNonQueryAsync().Wait();
        command.Reset();

        var registerTableCmd = $@"CREATE TABLE IF NOT EXISTS {REG_TABLE}
                                (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    UserId INTEGER,
                                    Name TEXT NOT NULL,
                                    Email TEXT,
                                    Description TEXT,
                                    Password TEXT NOT NULL,
                                    FOREIGN KEY (UserId) REFERENCES {ACC_TABLE}(Id)
                                )";

        command.CommandText = registerTableCmd;
        command.ExecuteNonQueryAsync().Wait();
        command.Reset();
    }

    #region Dispose
    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_connection.State != System.Data.ConnectionState.Closed)
                _connection.Close();
            _connection.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion
}