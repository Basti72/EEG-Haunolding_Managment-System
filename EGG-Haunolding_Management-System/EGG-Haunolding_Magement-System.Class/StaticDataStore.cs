using System.Text;

namespace EGG_Haunolding_Management_System.Class
{
    public class StaticDataStore : IDataStore
    {
        public List<DataItem> _data { get; set; }

        public StaticDataStore(List<DataItem> data)
        {
            _data = data;
        }

        public List<DataItem> GetAllDataByOrigin(string origin)
        {
            return GetDataByOrigin(origin);
        }

        public DataItem? GetCurrentDataByOrigin(string origin)
        {
            List<DataItem> data = GetDataByOrigin(origin);

            return data.Where(x => x.Origin == origin)
                .OrderByDescending(x => x.Time)
                .FirstOrDefault();
        }

        private List<DataItem> GetDataByOrigin(string origin)
        {
            List<DataItem> data = new List<DataItem>();
            for (int i = 0; i < _data.Count; i++)
            {
                if (_data[i].Origin == origin)
                    data.Add(_data[i]);
            }

            return data;
        }

        public List<DataItem> GetAllData()
        {
            return _data;
        }

        public string[] GetOrigins()
        {
            List<string> origins = new List<string>();

            for(int i = 0; i < _data.Count; i++)
            {
                bool found = false;
                for(int j = 0; j < origins.Count; j++)
                {
                    if (_data[i].Origin == origins[j])
                        found = true;
                }
                if(!found)
                    origins.Add(_data[i].Origin);
            }

            return origins.ToArray();
        }

        public List<DataItem> GetCurrentDataByOriginByDay(string origin, DateTime time, int compressionLevel)
        {
            List<DataItem> data = new List<DataItem>();
            for (int i = 0; i < _data.Count; i++)
            {
                if (_data[i].Origin == origin && _data[i].Time.Date == time.Date && _data[i].CompressionLevel == compressionLevel)
                    data.Add(_data[i]);
            }

            return data;
        }

        public void DeleteAllDataByOriginByDay(string origin, DateTime time, int compressionLevel)
        {
            for (int i = 0; i < _data.Count; i++)
            {
                if (_data[i].Origin == origin && _data[i].Time.Date == time.Date && _data[i].CompressionLevel == compressionLevel)
                    _data.Remove(_data[i]);
            }
        }

        public void InsertData(DataItem item)
        {
            _data.Add(item);
        }

        public List<DataItem> GetAllLastDataByOrigin(string origin, int amount)
        {
            List<DataItem> data = GetDataByOrigin(origin);

            return data.Where(x => x.Origin == origin)
                .OrderByDescending(x => x.Time)
                .Take(amount)
                .ToList();
        }
    }
}