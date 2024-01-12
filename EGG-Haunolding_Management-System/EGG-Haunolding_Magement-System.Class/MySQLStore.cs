using MySqlConnector;
using Dapper;

namespace EGG_Haunolding_Management_System.Class
{
    public class MySQLStore : IDataStore, IMQTTCom
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

            return connection.Query<DataItem>("SELECT * FROM Data").ToList();
        }

        public List<DataItem> GetAllDataByOrigin(string origin)
        {
            using MySqlConnection connection = new(ConnectionString);

            var entry = new {Origin = origin};

            return connection.Query<DataItem>("SELECT * FROM Data WHERE Origin = @Origin", entry).ToList();
        }

        public DataItem? GetCurrentDataByOrigin(string origin)
        {
            using MySqlConnection connection = new(ConnectionString);

            var entry = new { Origin = origin };

            return connection.QueryFirstOrDefault<DataItem>("SELECT * FROM Data WHERE Origin = @Origin ORDER BY TIME DESC", entry);
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

            return connection.Query<DataItem>("SELECT * FROM Data WHERE Origin = @Origin AND DATE(time) = @Time AND CompressionLevel = @CompressionLevel", entry).ToList();
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

            connection.Execute("DELETE FROM Data WHERE Origin = @Origin AND DATE(time) = @Time AND CompressionLevel = @CompressionLevel", entry);
        }

        public string[] GetOrigins()
        {
            using MySqlConnection connection = new(ConnectionString);

            return connection.Query<string>("SELECT DISTINCT Origin FROM Data").ToArray();
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

            connection.Execute("INSERT INTO Data VALUES (@Origin, @Time, @Saldo, @SaldoAvg, @CompressionLevel)", entry);
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

            return connection.Query<DataItem>("SELECT * FROM data WHERE Origin = @Origin AND CompressionLevel = @CompressionLevel ORDER BY TIME DESC LIMIT @Amount", entry).ToList();
        }
    }
}