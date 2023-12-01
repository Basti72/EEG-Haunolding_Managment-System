using EGG_Haunolding_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EGG_Haunolding_Management_System.Controllers
{
    public class DashboardController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}