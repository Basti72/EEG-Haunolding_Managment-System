using EGG_Haunolding_Management_System.Class;
using EGG_Haunolding_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EGG_Haunolding_Management_System.Controllers
{
    public class TopicController : Controller
    {
        private ITopicStore TopicStore { get; }
        public TopicController(ITopicStore topicStore)
        {
            TopicStore = topicStore;
        }

        public IActionResult Index()
        {
            List<TopicItem> topics = TopicStore.GetAllTopics();

            return View(new TopicViewModel { Topics = topics });
        }

        public IActionResult Delete(int id)
        {
            TopicStore.DeleteTopic(id);

            return RedirectToAction("Index");
        }

        public IActionResult NewTopic()
        {
            return View("NewTopic", new NewTopicViewModel { Topic = "zaehlerbroadcast/"});
        }

        public IActionResult AddTopic(NewTopicViewModel model)
        {
            if(!ModelState.IsValid)
                return View("NewTopic", model);

            string prefix = "zaehlerbroadcast/";
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