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
            List<string> origins = new List<string>();
            foreach(TopicItem topicItem in topicItems)
            {
                origins.Add(topicItem.Topic);
            }
            if (origins == null)
                return View("NoDataFound");
            if (name == null || !origins.Contains(name))
                name = origins[0];
            DashboardViewModel model = GetData(name);
            if (model == null)
                return View("NoDataFound");
            model.Origins = origins;
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
            List<TopicItem> topicItems = m_TopicStore.GetTopicItemsByUser(User.Identity.Name);
            if (topicItems.Count == 1 && topicItems[0].Id == 1)
            {
                topicItems = m_TopicStore.GetAllTopics();
                topicItems.RemoveAt(0);
            }
            List<string> origins = new List<string>();
            foreach (TopicItem topicItem in topicItems)
            {
                origins.Add(topicItem.Topic);
            }
            data.Origins = origins;
            return Json(data);
        }

        public IActionResult GetLatestData(string origin)
        {
            var data = GetData(origin);
            if (data == null)
                return null;
            List<TopicItem> topicItems = m_TopicStore.GetTopicItemsByUser(User.Identity.Name);
            if (topicItems.Count == 1 && topicItems[0].Id == 1)
            {
                topicItems = m_TopicStore.GetAllTopics();
                topicItems.RemoveAt(0);
            }
            List<string> origins = new List<string>();
            foreach (TopicItem topicItem in topicItems)
            {
                origins.Add(topicItem.Topic);
            }
            data.Origins = origins;
            return Json(data);
        }

        public IActionResult GetTimeFrameData(string timeframe, string origin)
        {
            Console.WriteLine($"TimeFrame: {timeframe}");
            List<TopicItem> topicItems = m_TopicStore.GetTopicItemsByUser(User.Identity.Name);
            if (topicItems.Count == 1 && topicItems[0].Id == 1)
            {
                topicItems = m_TopicStore.GetAllTopics();
                topicItems.RemoveAt(0);
            }
            List<string> origins = new List<string>();
            foreach (TopicItem topicItem in topicItems)
            {
                origins.Add(topicItem.Topic);
            }
            var data = GetDataFromTimeFrame(timeframe, origin);
            if (data == null)
                return null;
            data.Origins = origins;
            return Json(data);
        }

        private DashboardViewModel GetDataFromTimeFrame(string timeframe, string origin)
        {
            List<DataItem>? data = new List<DataItem>();
            switch(timeframe)
            {
                case "1": data = m_DataStore.GetDataByTime(origin, 0, DateTime.Now.Date.AddHours(DateTime.Now.Hour).AddHours(-1), DateTime.Now); break;
                case "2": data = m_DataStore.GetDataByTime(origin, 0, DateTime.Today, DateTime.Now); break;
                case "3": break;
                case "4": break;
                case "5": break;
                case "6": break;
                case "7": data = m_DataStore.GetDataByTime(origin, 0, DateTime.Now, DateTime.MinValue); break;
            }
            if (data == null)
                return null;
            // Assign data from DB to local variables
            var times = new List<string>();
            var values = new List<int>();
            for (int i = 0; i < data.Count; i++)
            {
                times.Add(data[i].Time.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add(data[i].Saldo);
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

    }

}