namespace EGG_Haunolding_Management_System.Class
{
    public interface IDataStore
    {
        List<DataItem> GetAllData();
        List<DataItem> GetAllDataByOrigin(string origin);
        List<DataItem> GetCurrentDataByOriginByDay(string origin, DateTime time, int compressionLevel);
        void DeleteAllDataByOriginByDay(string origin, DateTime time, int compressionLevel);
        void InsertData(DataItem item);
        DataItem? GetCurrentDataByOrigin(string origin);
        string[] GetOrigins();
    }
}