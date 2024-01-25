using EGG_Haunolding_Management_System.Class;
using EGG_Haunolding_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EGG_Haunolding_Management_System.Controllers
{
    public class UserController : Controller
    {
        private IUserStore UserStore { get; }
        public UserController(IUserStore userStore)
        {
            UserStore = userStore;
        }

        public IActionResult Index()
        {
            List<UserItem> users = UserStore.GetUsers();

            return View(new UserViewModel { Users = users });
        }

        public IActionResult Delete(string username, UserViewModel model)
        {
            UserStore.DeleteUser(username);

            return RedirectToAction("Index");
        }

        public IActionResult Edit(string username)
        {
            return RedirectToAction("Index", "Edit", new { userId = username });
        }
    }
}