using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace ServerLibrary
{
    class SqliteDatabase
    {
        public static UserModel GetUser(string username)
        {
            using (IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
            {
                try
                {
                    return conn.QueryFirst<UserModel>("SELECT * FROM User WHERE username = @Username",
                        new { Username = username });
                }
                catch (Exception ex)
                {
                    throw new Exception($"Fail! User {username} doesn't exist", ex);
                }
            }
        }

        public static void SaveUser(UserModel user)
        {
            using (IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
            {
                try
                {
                    conn.Execute("INSERT INTO User(username, password) VALUES (@Username, @Password)",
                        new {Username = user.Username, Password = user.Password});
                }
                catch (Exception ex)
                {
                    throw new Exception($"Fail! User {user.Username} already exists", ex);
                }
            }
        }

        public static void UpdateUser(UserModel user)
        {
            using (IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
            {
                try
                {
                    conn.Execute("UPDATE User SET password = @Password WHERE id = @Id", new { Id = user.Id, Password = user.Password });
                }
                catch (Exception ex)
                {
                    throw new Exception($"Fail! User {user.Username} not found", ex);
                }
            }
        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
