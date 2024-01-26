using EGG_Haunolding_Management_System.Class;
using EGG_Haunolding_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EGG_Haunolding_Management_System.Controllers
{
    public class EditController : Controller
    {
        private IUserStore UserStore { get; }
        public EditController(IUserStore userStore)
        {
            UserStore = userStore;
        }

        public IActionResult Index(string userId)
        {
            UserItem user = UserStore.GetUser(userId);

            return View(new EditViewModel { OriginalUsername = user.Username, Username = user.Username, Role = user.Role });
        }

        public IActionResult DoEdit(EditViewModel model)
        {

            bool userUpdated = UserStore.UpdateUser(model.OriginalUsername, model.Username, model.Role);
            
            if (!userUpdated) 
            {
                ModelState.AddModelError("UsernameAlreadyExists", "Der Benutzername existiert bereits");
                return View("Index", model);
            }

            return RedirectToAction("Index", "User");
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