using EGG_Haunolding_Management_System.Class;
using EGG_Haunolding_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Diagnostics;

namespace EGG_Haunolding_Management_System.Controllers
{
    public class LoginController : Controller
    {
        private AuthenticationLogic UserData { get; set; }
        private readonly IConfiguration _configuration;
        public LoginController(AuthenticationLogic userData, IConfiguration config)
        {
            UserData = userData;
            _configuration = config;
        }

        public IActionResult Index()
        {
            if (!DatabaseConnectionWorking())
                return View("DatabaseNotWorking");

            if (HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View(new LoginViewModel());
        }

        private bool DatabaseConnectionWorking()
        {
            using MySqlConnection connection = new(_configuration.GetConnectionString("Db"));
            try
            {
                connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

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