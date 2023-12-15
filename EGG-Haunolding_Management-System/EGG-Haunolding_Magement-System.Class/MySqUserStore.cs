using Dapper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGG_Haunolding_Management_System.Class
{
    public class MySqUserStore : IUserStore
    {
        MySqlConnection conn;
        public MySqUserStore (string path)
        {
            conn = new MySqlConnection (path);
        }

        public void AddUser(string username, string password)
        {
            string hash;
            hash = Password.CreateHash(password, out string salt);
            conn.ExecuteScalar($"Insert into users ({username},{hash},{salt},Admin");
        }

        public void ChangePassword(string username, string oldpw, string newpw, string confnewpw)
        {
            throw new NotImplementedException();
        }

        public UserItem GetUser(string username, string password)
        {
            var entry = new
            {
                Username = username
            };

            UserItem item = conn.QueryFirstOrDefault<UserItem>("SELECT * FROM users WHERE Username = @Username", entry);

            if (!Password.DoesPasswordMatch(password, item.Hash, item.Salt))
                item = null;


            return item;
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
