using EGG_Haunolding_Management_System.Class;
using EGG_Haunolding_Management_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace EGG_Haunolding_Management_System.Controllers
{
    public class LoginController : Controller
    {
        static string path = Directory.GetParent(Directory.GetCurrentDirectory()).FullName + "\\Resources\\ConnectionStringExtern.txt";

        static readonly IUserStore userStore = new MySQLUserStore(path: path);
        Authenticator Auth = new Authenticator(userStore);
        public IActionResult Login()
        {
            return View();
        }
        public async Task<IActionResult> Logon(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Login));
            }
            if (!await Auth.Login(model.Username!, model.Password!, HttpContext))
            {
                ModelState.AddModelError("", "Der Benutzername oder das Passwort ist nicht gültig!");
                return View(nameof(Login));
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
