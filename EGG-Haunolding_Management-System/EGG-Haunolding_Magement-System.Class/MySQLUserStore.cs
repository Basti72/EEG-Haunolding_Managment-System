using Dapper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGG_Haunolding_Management_System.Class
{
    public class MySQLUserStore : IUserStore
    {
    private readonly string ConnectionString;
        MySqlConnection conn;
        public MySQLUserStore (string path)
        {
            ConnectionString = File.ReadAllText(path);
        }

        public void AddUser(string username, string password)
        {
            using MySqlConnection conn = new MySqlConnection(ConnectionString);
            string hash = Password.CreateHash(password, out string salt);
            var entry = new
            {
                Username = username,
                Hash = hash,
                Salt = salt,
                Role = "Admin"
            };
            conn.Execute("INSERT INTO users(username, hash, salt, role) VALUES (@Username, @Hash, @Salt, @Role)", entry);
        }

        public void ChangePassword(string username, string oldpw, string newpw, string confnewpw)
        {
            throw new NotImplementedException();
        }

        public UserItem? GetUser(string username, string password)
        {
            var entry = new
            {
                Username = username,
                Password = password
            };
            return conn.QueryFirstOrDefault<UserItem>("SELECT * FROM User WHERE Username = @Username AND Password = @Password", entry);
        }
    

    public IEnumerable<UserItem> GetUsers()
        {
            IEnumerable<UserItem> items = null;

            var entry = new UserItem();

            try
            {
                items = conn.Query<UserItem>($"SELECT * FROM users", entry);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return items;
        }

        public void RemoveUser(string username)
        {
            throw new NotImplementedException();
        }
    }
}
