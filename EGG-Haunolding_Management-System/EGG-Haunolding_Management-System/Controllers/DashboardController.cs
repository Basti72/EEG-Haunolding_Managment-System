using EGG_Haunolding_Magement_System.Class;
using EGG_Haunolding_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EGG_Haunolding_Management_System.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDataStore _dataStore;

        public DashboardController(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}