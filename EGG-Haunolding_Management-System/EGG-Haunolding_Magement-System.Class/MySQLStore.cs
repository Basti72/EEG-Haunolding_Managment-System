using MySqlConnector;
using Dapper;
using System.Data;

namespace EGG_Haunolding_Management_System.Class
{
    public class MySQLStore : IDataStore, IMQTTCom, IUserStore, ITopicStore
    {
        private readonly string ConnectionString;
        private Dictionary<string, DateTime> LastEntryByOrigin;
        public MySQLStore(string path)
        {
            ConnectionString = File.ReadAllText(path);
            LastEntryByOrigin = new Dictionary<string, DateTime>();
        }

        public List<DataItem> GetAllData()
        {
            using MySqlConnection connection = new(ConnectionString);

            try
            {
                return connection.Query<DataItem>("SELECT * FROM Data").ToList();
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public List<DataItem> GetAllDataByOrigin(string origin)
        {
            using MySqlConnection connection = new(ConnectionString);

            var entry = new {Origin = origin};

            try
            {
                return connection.Query<DataItem>("SELECT * FROM Data WHERE Origin = @Origin", entry).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public DataItem? GetCurrentDataByOrigin(string origin)
        {
            using MySqlConnection connection = new(ConnectionString);

            var entry = new { Origin = origin };

            try
            {
                return connection.QueryFirstOrDefault<DataItem>("SELECT * FROM Data WHERE Origin = @Origin ORDER BY TIME DESC", entry);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public List<DataItem> GetCurrentDataByOriginByDay(string origin, DateTime time, int compressionLevel)
        {
            using MySqlConnection connection = new(ConnectionString);

            var entry = new
            {
                Origin = origin,
                Time = time.ToString("yyyy-MM-dd"),
                CompressionLevel = compressionLevel
            };

            try
            {
                return connection.Query<DataItem>("SELECT * FROM Data WHERE Origin = @Origin AND DATE(time) = @Time AND CompressionLevel = @CompressionLevel", entry).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public void DeleteAllDataByOriginByDay(string origin, DateTime time, int compressionLevel)
        {
            using MySqlConnection connection = new(ConnectionString);

            var entry = new
            {
                Origin = origin,
                Time = time.ToString("yyyy-MM-dd"),
                CompressionLevel = compressionLevel
            };

            try
            {
                connection.Execute("DELETE FROM Data WHERE Origin = @Origin AND DATE(time) = @Time AND CompressionLevel = @CompressionLevel", entry);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public string[] GetOrigins()
        {
            using MySqlConnection connection = new(ConnectionString);

            try
            {
                return connection.Query<string>("SELECT DISTINCT Origin FROM Data").ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public void InsertIntoDatabase(DataItem item)
        {
            using MySqlConnection connection = new(ConnectionString);

            var entry = new
            {
                Origin = item.Origin,
                Time = item.Time.ToString("yyyy-MM-dd HH:mm:ss"),
                Saldo = item.Saldo,
                SaldoAvg = item.SaldoAvg
            };

            if (LastEntryByOrigin.ContainsKey(item.Origin))
            {
                if (LastEntryByOrigin[item.Origin] == item.Time)
                {
                    return;
                }

                else
                {
                    LastEntryByOrigin[item.Origin] = item.Time;
                }
            }
            else
            {
                LastEntryByOrigin[item.Origin] = item.Time;
            }

            try
            {
                connection.Execute("INSERT INTO Data(Origin, Time, Saldo, SaldoAvg) VALUES (@Origin, @Time, @Saldo, @SaldoAvg)", entry);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void InsertData(DataItem item)
        {
            using MySqlConnection connection = new(ConnectionString);

            var entry = new
            {
                Origin = item.Origin,
                Time = item.Time.ToString("yyyy-MM-dd HH:mm:ss"),
                Saldo = item.Saldo,
                SaldoAvg = item.SaldoAvg,
                CompressionLevel = item.CompressionLevel
            };

            try
            {
                connection.Execute("INSERT INTO Data VALUES (@Origin, @Time, @Saldo, @SaldoAvg, @CompressionLevel)", entry);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public List<DataItem> GetAllLastDataByOrigin(string origin, int amount, int compressionLevel)
        {
            using MySqlConnection connection = new(ConnectionString);

            var entry = new
            {
                Origin = origin,
                CompressionLevel = compressionLevel,
                Amount = amount,
            };

            try
            {
                return connection.Query<DataItem>("SELECT * FROM data WHERE Origin = @Origin AND CompressionLevel = @CompressionLevel ORDER BY TIME DESC LIMIT @Amount", entry).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public UserItem? GetUserWithPassword(string username, string password)
        {
            using MySqlConnection connection = new(ConnectionString);

            var entry = new
            {
                Username = username
            };

            UserItem item = connection.QueryFirstOrDefault<UserItem>("SELECT * FROM users WHERE Username = @Username", entry);

            if (item == null || !Util.DoesPasswordMatch(password, item.Hash, item.Salt))
                item = null;


            return item;
        }

        public bool AddUser(UserItem item)
        {
            using MySqlConnection connection = new(ConnectionString);

            var entry = new
            {
                Username = item.Username,
                Hash = item.Hash,
                Salt = item.Salt,
                Role = item.Role
            };

            try
            {
                connection.ExecuteScalar("INSERT INTO users VALUES(@Username, @Hash, @Salt, @Role)", entry);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public List<UserItem> GetUsers()
        {
            using MySqlConnection connection = new(ConnectionString);

            try
            {
                return connection.Query<UserItem>("SELECT * FROM users").ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public void DeleteUser(string username)
        {
            using MySqlConnection connection = new(ConnectionString);

            var entry = new
            {
                Username = username,
            };

            try
            {
                connection.ExecuteScalar("DELETE FROM users WHERE username = @Username", entry);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public UserItem GetUser(string username)
        {
            using MySqlConnection connection = new(ConnectionString);

            var entry = new
            {
                Username = username,
            };

            try
            {
                return connection.QueryFirstOrDefault<UserItem>("SELECT * FROM users WHERE username = @Username", entry);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public bool UpdateUser(string oldUsername, string newUsername, string role)
        {
            using MySqlConnection connection = new(ConnectionString);

            var entry = new
            {
                NewUsername = newUsername,
                OldUsername = oldUsername,
                Role = role
            };

            try
            {
                connection.ExecuteScalar("UPDATE users SET username = @NewUsername, ROLE = @Role WHERE Username = @OldUsername", entry);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public void UpdatePassword(string username, string hash, string salt)
        {
            using MySqlConnection connection = new(ConnectionString);

            var entry = new
            {
                Username = username,
                Hash = hash,
                Salt = salt
            };

            try
            {
                connection.ExecuteScalar("UPDATE users SET hash = @Hash, salt = @Salt WHERE username = @Username", entry);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        List<Topic> ITopicStore.GetAllTopics()
        {
            using MySqlConnection connection = new(ConnectionString);

            return connection.Query<Topic>("SELECT * FROM topics").ToList();
        }
    }
}
