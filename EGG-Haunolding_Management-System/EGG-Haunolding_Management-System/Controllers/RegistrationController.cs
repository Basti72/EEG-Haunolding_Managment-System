using EGG_Haunolding_Management_System.Class;
using EGG_Haunolding_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace EGG_Haunolding_Management_System.Controllers
{
    public class RegistrationController : Controller
    {
        private IUserStore UserStore { get; }
        private ITopicStore TopicStore { get; }
        private IConfiguration Config { get; }
        public RegistrationController(IUserStore userStore, IConfiguration config, ITopicStore topicStore)
        {
            UserStore = userStore;
            Config = config;
            TopicStore = topicStore;
        }

        public IActionResult Index()
        {
            return View(LoadModel());
        }

        public IActionResult DoRegistration(RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Index), LoadModel());
            }

            if (model.AccessAllTopics)
            {
                model.SelectedTopicIds = new List<int> { 1 };
            }

            string salt;
            string hash = Util.CreateHash(model.Password, out salt);

            if (!UserStore.AddUser(new UserItem(model.Username, hash, salt, model.Role)))
            {
                ModelState.AddModelError("", "This username already exits!");
                return View(nameof(Index), model);
            }

            foreach (int id in model.SelectedTopicIds)
                TopicStore.AddTopicToUser(model.Username, id);

            return RedirectToAction("Index", "User");
        }

        public IActionResult Import()
        {
            return View();
        }

        private RegistrationViewModel LoadModel()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();

            List<TopicItem> topics = TopicStore.GetAllTopics();
            topics.RemoveAt(0);

            foreach (TopicItem topicItem in topics)
                listItems.Add(new SelectListItem { Value = topicItem.Id.ToString(), Text = topicItem.Topic });

            return new RegistrationViewModel
            {
                AvailableTopics = listItems
            };
        }

        [HttpPost]
        public IActionResult UploadFile(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                // Überprüfen, ob die Datei eine .txt-Datei ist
                if (Path.GetExtension(file.FileName).ToLower() == ".txt")
                {
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        var fileContent = reader.ReadToEnd();
                        string[] fileLines = fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                        foreach(string line in fileLines)
                        {
                            string[] splitLine = line.Split(';');
                            string salt;
                            string hash = Util.CreateHash(splitLine[1], out salt);

                            if (!UserStore.AddUser(new UserItem(splitLine[0], hash, salt, splitLine[2])))
                            {
                                Console.WriteLine($"Could not insert User {splitLine[0]}");
                            }
                        }

                        return View(nameof(Index), new RegistrationViewModel());
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Bitte wählen Sie eine gültige .txt-Datei aus.");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Bitte wählen Sie eine Datei aus.");
            }
            return View("Import");
        }
    }
}