using System.Data.SQLite;
using XPassword.Database.Model;

namespace XPassword.Database.Data;

internal static class Builder
{
    internal static bool BuildEmailExist(SQLiteDataReader reader)
    {
        if (!reader.HasRows)
        {
            reader.Close();
            return false;
        }

        while (reader.Read())
        {
            if ((string)reader["Email"] != null)
            {
                reader.Close();
                return true;
            }
        }

        reader.Close();
        return false;
    }

    internal static List<Account> BuildAccounts(SQLiteDataReader reader)
    {
        var accounts = new List<Account>();

        if (!reader.HasRows)
        {
            reader.Close();
            return accounts;
        }

        while (reader.Read())
        {
            accounts.Add(new Account
            {
                Id = (long)reader["Id"],
                Email = (string)reader["Email"],
                Name = (string)reader["Name"],
                HPassword = (string)reader["HPassword"]
            });
        }

        reader.Close();
        return accounts;
    }

    internal static List<Register> BuildRegister(SQLiteDataReader reader)
    {
        var registers = new List<Register>();

        if (!reader.HasRows)
        {
            reader.Close();
            return registers;
        }

        while (reader.Read())
        {
            registers.Add(new Register
            {
                Id = (long)reader["Id"],
                UserId = (long)reader["UserId"],
                Name = (string)reader["Name"],
                Email = (string)reader["Email"],
                Description = (string)reader["Description"],
                Password = (string)reader["Password"]
            });
        }

        reader.Close();
        return registers;
    }
}