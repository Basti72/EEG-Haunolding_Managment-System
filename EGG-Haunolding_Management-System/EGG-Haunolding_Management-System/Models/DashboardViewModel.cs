namespace EGG_Haunolding_Management_System.Models
{
    public class DashboardViewModel
    {
        public List<string> Times { get; set; }
        public List<int> Values { get; set; }
        public string Origin { get; set; }

        public List<string> Origins { get; set; } = new List<string>();
    }
}