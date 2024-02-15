using EGG_Haunolding_Management_System.Class;
using EGG_Haunolding_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Globalization;

namespace EGG_Haunolding_Management_System.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDataStore m_DataStore;
        private readonly ITopicStore m_TopicStore;
        private readonly IConfiguration m_Config;
        public DashboardController(IDataStore dataStore, ITopicStore topicStore, IConfiguration config)
        {
            m_DataStore = dataStore;
            m_TopicStore = topicStore;
            m_Config = config;
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
            DashboardViewModel model = GetDataFromTimeFrame("2", name);
            if (model == null)
                return View("NoDataFound");
            model.Origins = origins;

            int refreshRate = m_Config.GetValue<int>("RefreshRate");
            ViewBag.RefreshRate = refreshRate;

            return View(model);
        }

        public IActionResult UpdateChart(string origin, string timeFrame)
        {
            var data = GetDataFromTimeFrame(timeFrame, origin);
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

        public IActionResult GetLatestData(string origin, string timeFrame)
        {
            Console.WriteLine($"timeFrame: {timeFrame}");
            var data = GetDataFromTimeFrame(timeFrame, origin);
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
            var times = new List<string>();
            var values = new List<int>();
            switch (timeframe)
            {
                case "1":
                    {
                        data = m_DataStore.GetDataByTime(origin, 0, DateTime.Now - TimeSpan.FromHours(1), DateTime.Now); 
                        if (data == null)
                            return null;
                        for (int i = 0; i < data.Count; i++)
                        {
                            times.Add(data[i].Time.ToString("dd-MM-yyyy HH:mm:ss"));
                            values.Add(data[i].Saldo);
                        }
                        break;
                    }
                case "2":
                    {
                        data = m_DataStore.GetDataByTime(origin, 0, DateTime.Today, DateTime.Now); 
                        if (data == null)
                            return null;
                        for (int i = 0; i < data.Count; i++)
                        {
                            times.Add(data[i].Time.ToString("dd-MM-yyyy HH:mm:ss"));
                            values.Add(data[i].Saldo);
                        }
                        break;
                    }
                case "3":
                    {
                        data = m_DataStore.GetDataByTime(origin, 0, DateTime.Now - TimeSpan.FromHours(24), DateTime.Now);
                        if (data == null)
                            return null;
                        for(int i = 0; i < 24; i++)
                        {
                            var currentDay = (DateTime.Now - TimeSpan.FromHours(23 - i)).Day;
                            var currentHour = (DateTime.Now - TimeSpan.FromHours(23-i)).Hour;
                            times.Add(currentHour.ToString() + ":00");

                            List<int> vals = new List<int>();
                            for(int j = 0; j < data.Count; j++)
                            {
                                if (data[j].Time.Hour == currentHour && data[j].Time.Day == currentDay)
                                    vals.Add(data[j].Saldo);
                            }
                            if (vals.Count < 1)
                                values.Add(0);
                            else
                            {
                                int x = vals.Sum();
                                values.Add(x / vals.Count);
                            }
                        }
                        break;
                    }
                case "4":
                    {
                        data = m_DataStore.GetDataByTime(origin, 0, DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek), DateTime.Now);
                        if (data == null)
                            return null;

                        int daysIntoWeek = (int)DateTime.Today.DayOfWeek;
                        for (int i = 0; i < daysIntoWeek; i++)
                        {
                            var currentWeekDay = ((DayOfWeek)i + 1);
                            CultureInfo germanCulture = new CultureInfo("de-DE");
                            times.Add(germanCulture.DateTimeFormat.GetDayName(currentWeekDay));

                            List<int> vals = new List<int>();
                            for(int j = 0; j < data.Count; j++)
                            {
                                if (data[j].Time.Date == DateTime.Today.AddDays(-(daysIntoWeek - i)+1))
                                    vals.Add(data[j].Saldo);
                            }
                            if (vals.Count < 1)
                                values.Add(0);
                            else
                            {
                                int x = vals.Sum();
                                values.Add(x / vals.Count);
                            }
                        }
                        break;
                    }
                case "5":
                    {
                        data = m_DataStore.GetDataByTime(origin, 0, DateTime.Now - TimeSpan.FromDays(30), DateTime.Now);
                        if (data == null)
                            return null;
                        for (int i = 0; i < 30; i++)
                        {
                            var currentDay = DateTime.Today.AddDays(-(30 - i)+1);
                            CultureInfo germanCulture = new CultureInfo("de-DE");
                            times.Add(currentDay.Date.ToString("d", germanCulture));

                            List<int> vals = new List<int>();
                            for (int j = 0; j < data.Count; j++)
                            {
                                if (data[j].Time.Date == currentDay.Date)
                                    vals.Add(data[j].Saldo);
                            }
                            if (vals.Count < 1)
                                values.Add(0);
                            else
                            {
                                int x = vals.Sum();
                                values.Add(x / vals.Count);
                            }
                        }
                        break;
                    }
                case "6":
                    {
                        data = m_DataStore.GetDataByTime(origin, 0, new DateTime(DateTime.Today.Year, 1, 1), DateTime.Now);
                        if (data == null)
                            return null;

                        DateTime firstDayOfYear = new DateTime(DateTime.Now.Year, 1, 1);
                        int daysIntoYear = (DateTime.Today - firstDayOfYear).Days + 1;
                        for (int i = 0; i < daysIntoYear; i++)
                        {
                            var currentDay = DateTime.Today.AddDays(-(daysIntoYear - i) + 1);
                            CultureInfo germanCulture = new CultureInfo("de-DE");
                            times.Add(currentDay.Date.ToString("d", germanCulture));

                            List<int> vals = new List<int>();
                            for (int j = 0; j < data.Count; j++)
                            {
                                if (data[j].Time.Date == currentDay.Date)
                                    vals.Add(data[j].Saldo);
                            }
                            if (vals.Count < 1)
                                values.Add(0);
                            else
                            {
                                int x = vals.Sum();
                                values.Add(x / vals.Count);
                            }
                        }
                        break;
                    }
                case "7":
                    {
                        data = m_DataStore.GetDataByTime(origin, 0, DateTime.MinValue, DateTime.Now);
                        if (data == null)
                            return null;

                        var startDate = data[0].Time.Date;
                        startDate = new DateTime(startDate.Year, startDate.Month, 1);
                        for (; startDate < DateTime.Today;)
                        {
                            times.Add(startDate.Month.ToString() + "/" + startDate.Year.ToString());

                            List<int> vals = new List<int>();
                            for(int j = 0; j < data.Count; j++)
                            {
                                if (data[j].Time.Year == startDate.Year && data[j].Time.Month == startDate.Month)
                                    vals.Add(data[j].Saldo);
                            }

                            if (vals.Count < 1)
                                values.Add(0);
                            else
                            {
                                int x = vals.Sum();
                                values.Add(x / vals.Count);
                            }

                            if (startDate.Month == 12)
                                startDate = new DateTime(startDate.Year + 1, 1, 1);
                            else
                                startDate = new DateTime(startDate.Year, startDate.Month + 1, 1);
                        }
                        break;
                    }
            }

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