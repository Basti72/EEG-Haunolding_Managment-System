namespace EGG_Haunolding_Magement_System.Class
{
    public interface IDataStore
    {
        List<DataItem> GetAllDataByOrigin(string origin);

        DataItem? GetCurrentDataByOrigin(string origin);
    }
}