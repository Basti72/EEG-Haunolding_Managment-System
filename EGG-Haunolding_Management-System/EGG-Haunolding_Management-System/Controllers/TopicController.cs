using EGG_Haunolding_Management_System.Class;
using EGG_Haunolding_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EGG_Haunolding_Management_System.Controllers
{
    public class TopicController : Controller
    {
        private ITopicStore TopicStore { get; }
        private IConfiguration Config { get; }
        public TopicController(ITopicStore topicStore, IConfiguration config)
        {
            TopicStore = topicStore;
            Config = config;
        }

        public IActionResult Index()
        {
            List<TopicItem> topics = TopicStore.GetAllTopics();
            string topic = Config.GetValue<string>("TopicName");

            return View(new TopicViewModel { Topics = topics, TopicName = topic });
        }

        public IActionResult Delete(int id)
        {
            TopicStore.DeleteTopic(id);

            return RedirectToAction("Index");
        }

        public IActionResult NewTopic()
        {
            string topic = Config.GetValue<string>("TopicName");
            return View("NewTopic", new NewTopicViewModel { Topic = topic });
        }

        public IActionResult AddTopic(NewTopicViewModel model)
        {
            if(!ModelState.IsValid)
                return View("NewTopic", model);

            string prefix = Config.GetValue<string>("TopicName");

            if(model.Topic.Length < prefix.Length || prefix != model.Topic.Substring(0, prefix.Length))
            {
                ModelState.AddModelError(string.Empty, $"Das Topic muss mit '{prefix}' beginnen.");
                return View("NewTopic", model);
            }

            string topic = model.Topic.Substring(prefix.Length);

            if(String.IsNullOrEmpty(topic))
            {
                ModelState.AddModelError(string.Empty, "Gib ein Topic ein!");
                return View("NewTopic", model);
            }

            if(TopicStore.CheckIfTopicExists(topic))
            {
                ModelState.AddModelError(string.Empty, "Dieses Topic existiert bereits!");
                return View("NewTopic", model);
            }
            
            TopicStore.AddTopic(new TopicItem(0, topic));

            return RedirectToAction("Index");
        }
    }
}