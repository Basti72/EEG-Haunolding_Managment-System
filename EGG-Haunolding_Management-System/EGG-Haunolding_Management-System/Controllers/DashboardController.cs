using EGG_Haunolding_Management_System.Class;
using EGG_Haunolding_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EGG_Haunolding_Management_System.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDataStore m_DataStore;
        public DashboardController(IDataStore dataStore, IUserStore userStore)
        {
            m_DataStore = dataStore;
        }

        public ActionResult Index()
        {
            // Get all data by origin
            List<List<DataItem>> dataList = new List<List<DataItem>>();
            var origins = m_DataStore.GetOrigins();
            foreach(var origin in origins)
                dataList.Add(m_DataStore.GetAllDataByOrigin(origin));
            // Assign data from DB to local variables
            var times = new List<string>();
            var values = new List<List<int>>();
            for(int i = 0; i < dataList.Count; i++)
            {
                List<int> ints = new List<int>();
                for(int j = 0; j < dataList[i].Count; j++)
                {
                    // Only assign time once
                    if (i == 0)
                        times.Add(dataList[i][j].Time.ToString("yyyy-MM-dd HH:mm:ss"));
                    ints.Add(dataList[i][j].Saldo);
                }
                values.Add(ints);
            }

            var model = new DashboardViewModel
            {
                Times = times,
                Values = values,
                Origins = origins.ToList()
            };

            return View(model);
        }
    }

}