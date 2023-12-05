// Docker Befehl: docker run -e MYSQL_ROOT_PASSWORD=1234 -e MYSQL_DATABASE=EGG-Haunolding_Management-System -p 3306:3306 mysql
// Create Table Befehl:
// CREATE TABLE `Data` (
//	`Origin` VARCHAR(50),
//	`Time` DATETIME,
//	`Saldo` INT NULL,
//	`SaldoAvg` INT NULL,
//    primary key(Origin, Time)
//)

using MySqlConnector;
using Dapper;

namespace EGG_Haunolding_Magement_System.Class
{
    public class MySQLDataStore : IDataStore
    {
        private readonly string ConnectionString;
        public MySQLDataStore(string path)
        {
            ConnectionString = File.ReadAllText(path);
        }

        public List<DataItem> GetAllData()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}