using Microsoft.Data.Sqlite;

namespace LofiWebPlayer.Biz.Manager.Configuration
{
    public class SqlConfiguraqtion
    {
        //internal static SqliteConnectionStringBuilder connectionStringBuilder = new SqliteConnectionStringBuilder()
        //{
        //    DataSource = "./Videos.db"
        //};

        internal static string CreationSqlCommand = "CREATE TABLE IF NOT EXISTS VideoTable ( VideoId INTEGER PRIMARY KEY, Title TEXT NOT NULL, Url TEXT NOT NULL);";
    }
}
