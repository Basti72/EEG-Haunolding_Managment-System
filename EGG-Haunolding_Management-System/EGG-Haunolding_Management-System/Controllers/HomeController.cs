using EGG_Haunolding_Management_System.Class;
using EGG_Haunolding_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EGG_Haunolding_Management_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataStore _dataStore;
        public HomeController(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public IActionResult Index()
        {
            List<Household> households = new List<Household>();
            string[] origins = _dataStore.GetOrigins();
            foreach(string origin in origins)
            {
                List<DataItem> item = _dataStore.GetAllLastDataByOrigin(origin, 1, 0);
                int value = item[0].Saldo;
                households.Add(new Household(origin, value));
            }

            var viewModel = new HomeViewModel
            {
                Households = households
            };

            viewModel.TotalValue = viewModel.Households.Sum(h => h.Value);

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetHouseholdData()
        {
            var households = new List<Household>();
            string[] origins = _dataStore.GetOrigins();
            foreach (string origin in origins)
            {
                List<DataItem> item = _dataStore.GetAllLastDataByOrigin(origin, 1, 0);
                int value = item[0].Saldo;
                households.Add(new Household(origin, value));
            }

            var totalValue = households.Sum(h => h.Value);
            return Json(new { TotalValue = totalValue, Households = households });
        }
    }

}