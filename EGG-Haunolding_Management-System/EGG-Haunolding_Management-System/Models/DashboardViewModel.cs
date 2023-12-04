namespace EGG_Haunolding_Management_System.Models
{
    public class DashboardViewModel
    {
        public string Title { get; set; }
        public string Origin { get; set; }
        public List<DataPoint> TimeAndValues { get; set; }

    }

    public class DataPoint
    {
        public long X { get; set; }
        public int Y { get; set; }
    }
}