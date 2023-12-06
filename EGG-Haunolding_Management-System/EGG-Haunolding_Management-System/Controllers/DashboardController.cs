using EGG_Haunolding_Magement_System.Class;
using EGG_Haunolding_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EGG_Haunolding_Management_System.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDataStore m_DataStore;
        public DashboardController(IDataStore dataStore)
        {
            m_DataStore = dataStore;
        }

        public ActionResult Index()
        {
            var data1 = m_DataStore.GetAllDataByOrigin("Oberndorfer");
            var data2 = m_DataStore.GetAllDataByOrigin("Bell");
            var data3 = m_DataStore.GetAllDataByOrigin("Roider");
            List<string> times = new List<string>();
            List<List<int>> values = new List<List<int>>();
            List<int> valueTemp = new List<int>();
            foreach (var item in data1)
            {
                times.Add(item.Time.ToString("yyyy-MM-dd HH:mm:ss"));
                valueTemp.Add(item.Saldo);
            }
            values.Add(valueTemp);
            values.Clear();

            foreach (var item in data2)
            {
                valueTemp.Add(item.Saldo);
            }
            values.Add(valueTemp);
            values.Clear();

            foreach (var item in data3)
            {
                valueTemp.Add(item.Saldo);
            }
            values.Add(valueTemp);
            values.Clear();

            var model = new DashboardViewModel
            {
                Times = times,
                Values = new List<int>{43, 50, 47}
            };

            return View(model);
        }
    }

}