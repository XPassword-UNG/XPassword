using System.Data.SQLite;
using XPassword.Database.Model;

namespace XPassword.Database.Data;

internal static class Builder
{
    internal static bool BuildEmailExist(SQLiteDataReader reader)
    {
        if (!reader.HasRows) 
            return false;

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

    internal static List<Account> BuildAccount(SQLiteDataReader reader)
    {
        var accounts = new List<Account>();

        if (!reader.HasRows)
            return accounts;

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
}