using EGG_Haunolding_Management_System.Class;
using EGG_Haunolding_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EGG_Haunolding_Management_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataStore _dataStore;
        private readonly ITopicStore _topicStore;
        private readonly IConfiguration _config;
        public HomeController(IDataStore dataStore, ITopicStore topicStore, IConfiguration config)
        {
            _dataStore = dataStore;
            _topicStore = topicStore;
            _config = config;
        }

        public IActionResult Index()
        {
            List<Household> households = new List<Household>();
            List<TopicItem> topicItems = _topicStore.GetTopicItemsByUser(User.Identity.Name);
            if(topicItems.Count == 1 && topicItems[0].Id == 1)
            {
                topicItems = _topicStore.GetAllTopics();
                topicItems.RemoveAt(0);
            }
                
            foreach (TopicItem topicItem in topicItems)
            {
                List<DataItem> item = _dataStore.GetAllLastDataByOrigin(topicItem.Topic, 1, 0);
                int value = 0;
                if(item.Count != 0)
                    value = item[0].Saldo;

                households.Add(new Household(topicItem.Topic, value));
            }

            var viewModel = new HomeViewModel
            {
                Households = households
            };

            viewModel.TotalValue = viewModel.Households.Sum(h => h.Value);

            int refreshRate = _config.GetValue<int>("RefreshRate");
            ViewBag.RefreshRate = refreshRate;

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetHouseholdData()
        {
            List<Household> households = new List<Household>();
            List<TopicItem> topicItems = _topicStore.GetTopicItemsByUser(User.Identity.Name);
            if (topicItems.Count == 1 && topicItems[0].Id == 1)
            {
                topicItems = _topicStore.GetAllTopics();
                topicItems.RemoveAt(0);
            }

            foreach (TopicItem topicItem in topicItems)
            {
                List<DataItem> item = _dataStore.GetAllLastDataByOrigin(topicItem.Topic, 1, 0);
                int value = 0;
                if (item.Count != 0)
                    value = item[0].Saldo;
                households.Add(new Household(topicItem.Topic, value));
            }

            var totalValue = households.Sum(h => h.Value);
            return Json(new { TotalValue = totalValue, Households = households });
        }
    }

}