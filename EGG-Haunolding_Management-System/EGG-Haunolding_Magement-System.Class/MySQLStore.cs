using MySqlConnector;
using Dapper;

namespace EGG_Haunolding_Management_System.Class
{
    public class MySQLStore : IDataStore, IMQTTCom
    {
        private readonly string ConnectionString;
        public MySQLStore(string path)
        {
            ConnectionString = File.ReadAllText(path);
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

            connection.Execute("INSERT INTO Data VALUES (@Origin, @Time, @Saldo, @SaldoAvg)", entry);
        }
    }
}