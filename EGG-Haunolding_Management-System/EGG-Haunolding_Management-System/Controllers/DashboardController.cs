using EGG_Haunolding_Management_System.Class;
using EGG_Haunolding_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace EGG_Haunolding_Management_System.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDataStore m_DataStore;
        private readonly ITopicStore m_TopicStore;
        private List<string> m_Origins;
        public DashboardController(IDataStore dataStore, ITopicStore topicStore)
        {
            m_DataStore = dataStore;
            m_TopicStore = topicStore;
        }

        public ActionResult Index(string name)
        {
            List<TopicItem> topicItems = m_TopicStore.GetTopicItemsByUser(User.Identity.Name);
            if (topicItems.Count == 1 && topicItems[0].Id == 1)
            {
                topicItems = m_TopicStore.GetAllTopics();
                topicItems.RemoveAt(0);
            }
            foreach(TopicItem topicItem in topicItems)
            {
                m_Origins.Add(topicItem.Topic);
            }
            if (m_Origins == null)
                return View("NoDataFound");
            if (name == null || !m_Origins.Contains(name))
                name = m_Origins[0];
            DashboardViewModel model = GetData(name);
            if (model == null)
                return View("NoDataFound");
            model.Origins = m_Origins;
            return View(model);
        }

        private DashboardViewModel GetData(string origin)
        {
            var dataList = m_DataStore.GetAllLastDataByOrigin(origin, 100, 0);
            if(dataList == null)
                return null;
            // Assign data from DB to local variables
            var times = new List<string>();
            var values = new List<int>();
            for (int i = 0; i < dataList.Count; i++)
            {
                times.Add(dataList[i].Time.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add(dataList[i].Saldo);
            }
            times.Reverse();
            values.Reverse();
            var model = new DashboardViewModel
            {
                Times = times,
                Values = values,
                Origin = origin
            };
            return model;
        }

        public IActionResult UpdateChart(string origin)
        {
            var data = GetData(origin);
            if (data == null) 
                return View("NoDataFound");
            data.Origins = m_Origins;
            return Json(data);
        }

        public IActionResult GetLatestData(string origin)
        {
            var data = GetData(origin);
            if (data == null)
                return null;
            data.Origins = m_Origins;
            return Json(data);
        }
    }

}