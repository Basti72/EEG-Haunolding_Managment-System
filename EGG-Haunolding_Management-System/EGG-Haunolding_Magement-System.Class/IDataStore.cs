namespace EGG_Haunolding_Management_System.Class
{
    public interface IDataStore
    {
        List<DataItem> GetAllData();
        List<DataItem> GetAllDataByOrigin(string origin);
        List<DataItem> GetCurrentDataByOriginByDay(string origin, DateTime time, int compressionLevel);
        List<DataItem> GetAllLastDataByOrigin(string origin, int amount, int compressionLevel);
        List<DataItem> GetDataByTime(string origin, int compressionLevel, DateTime startDate, DateTime endDate);
        void DeleteAllDataByOriginByDay(string origin, DateTime time, int compressionLevel);
        void InsertData(DataItem item);
        DataItem? GetCurrentDataByOrigin(string origin);
        string[] GetOrigins();
    }
}