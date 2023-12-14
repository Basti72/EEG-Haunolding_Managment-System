using Microsoft.Extensions.Hosting;

namespace EGG_Haunolding_Management_System.Class
{
    public class DBBackroundService : BackgroundService
    {
        private readonly IDataStore _dataStore;
        public DBBackroundService(IDataStore datastore)
        {
            _dataStore = datastore;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //Console.WriteLine("Test");
                await Task.Delay(1000);
            }
        }
    }
}