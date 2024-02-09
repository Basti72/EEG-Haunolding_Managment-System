using EGG_Haunolding_Management_System.Class;
using EGG_Haunolding_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EGG_Haunolding_Management_System.Controllers
{
    public class LoginController : Controller
    {
        private AuthenticationLogic UserData { get; set; }
        public LoginController(AuthenticationLogic userData)
        {
            UserData = userData;
        }

        public IActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View(new LoginViewModel());
        }

        public async Task<IActionResult> DoLogin(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Index));
            }

            if (!await UserData.Login(model.Username, model.Password, HttpContext))
            {
                ModelState.AddModelError("", "Der Benutzername oder das Passwort wurde nicht gefunden!");
                return View(nameof(Index));
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await UserData.Logout(HttpContext);
            return RedirectToAction(nameof(LoginController.Index), "Login");
        }
    }
}