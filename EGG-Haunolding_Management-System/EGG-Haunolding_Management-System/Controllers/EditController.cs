using EGG_Haunolding_Management_System.Class;
using EGG_Haunolding_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace EGG_Haunolding_Management_System.Controllers
{
    public class EditController : Controller
    {
        private IUserStore UserStore { get; }
        private ITopicStore TopicStore { get; }
        public EditController(IUserStore userStore, ITopicStore topicStore)
        {
            UserStore = userStore;
            TopicStore = topicStore;
        }

        public IActionResult Index(string userId)
        {
            return View(LoadModel(userId));
        }

        public IActionResult DoEdit(EditViewModel model)
        {
            if(!ModelState.IsValid)
                return View(LoadModel(model.OriginalUsername));

            if(model.AccessAllTopics)
                model.SelectedTopicIds = new List<int> { 1 };

            bool userUpdated = UserStore.UpdateUser(model.OriginalUsername, model.Username, model.Role);
            
            if (!userUpdated) 
            {
                ModelState.AddModelError("UsernameAlreadyExists", "Der Benutzername existiert bereits");
                return View("Index", LoadModel(model.OriginalUsername));
            }

            TopicStore.RemoveAllTopicsFromUser(model.OriginalUsername);

            foreach (int id in model.SelectedTopicIds)
                TopicStore.AddTopicToUser(model.Username, id);

            return RedirectToAction("Index", "User");
        }

        private EditViewModel LoadModel(string userId)
        {
            EditViewModel model = new EditViewModel();

            UserItem user = UserStore.GetUser(userId);
            model.OriginalUsername = user.Username;
            model.Username = user.Username;
            model.Role = user.Role;

            List<TopicItem> topics = TopicStore.GetAllTopics();
            topics.RemoveAt(0);

            List<SelectListItem> topicsSelectListItem = new List<SelectListItem>();
            foreach (TopicItem topicItem in topics)
                topicsSelectListItem.Add(new SelectListItem { Value = topicItem.Id.ToString(), Text = topicItem.Topic });
            model.AvailableTopics = topicsSelectListItem;

            List<int> topicIdsByUser = TopicStore.GetTopicsByUser(userId);
            if (topicIdsByUser.Count == 1 && topicIdsByUser[0] == 1)
                model.AccessAllTopics = true;

            else
                model.AccessAllTopics = false;
                
            model.SelectedTopicIds = topicIdsByUser;

            return model;
        }

        public IActionResult ChangePassword(string username)
        {
            return View("ChangePassword", new ChangePasswordViewModel { Username = username });
        }

        public IActionResult SubmitPassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ChangePassword", model);
            }

            string salt;
            string hash = Util.CreateHash(model.NewPassword, out salt);

            UserStore.UpdatePassword(model.Username, hash, salt);

            return RedirectToAction("Index", "User");
        }
    }
}