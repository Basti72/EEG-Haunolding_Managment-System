using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace EGG_Haunolding_Management_System.Class
{
    public class DBBackroundService : BackgroundService
    {
        private readonly IDataStore _dataStore;
        private readonly IConfiguration _configuration;
        public DBBackroundService(IDataStore datastore, IConfiguration config)
        {
            _dataStore = datastore;
            _configuration = config;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                string[] origins = _dataStore.GetOrigins();
                int compressionDays = _configuration.GetValue<int>("CompressionLevel1");
                DateTime compressionDay = DateTime.Now.AddDays(compressionDays * -1);

                foreach (string origin in origins)
                {
                    List<DataItem> data = _dataStore.GetCurrentDataByOriginByDay(origin, compressionDay, 0);

                    if(data.Count == 0)
                        break;

                    int saldoAvg = (int)data.Average(x => x.Saldo);

                    DataItem item = new DataItem();
                    item.Origin = origin;
                    item.Time = compressionDay;
                    item.Saldo = saldoAvg;
                    item.SaldoAvg = saldoAvg;
                    item.CompressionLevel = 1;
                    _dataStore.InsertData(item);
                    _dataStore.DeleteAllDataByOriginByDay(origin, compressionDay, 0);
                }

                await Task.Delay(TimeSpan.FromHours(1));
            }
        }
    }
}