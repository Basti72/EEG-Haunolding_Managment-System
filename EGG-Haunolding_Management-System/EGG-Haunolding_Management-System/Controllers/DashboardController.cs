﻿using EGG_Haunolding_Management_System.Class;
using EGG_Haunolding_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EGG_Haunolding_Management_System.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDataStore m_DataStore;
        public DashboardController(IDataStore dataStore, IUserStore userStore)
        {
            m_DataStore = dataStore;
        }

        public ActionResult Index(string name)
        {
            var origins = m_DataStore.GetOrigins();
            if (name == null)
                name = origins[0];
            DashboardViewModel model = GetData(name);
            model.Origins = origins.ToList();
            return View(model);
        }

        private DashboardViewModel GetData(string origin)
        {
            var dataList = m_DataStore.GetAllLastDataByOrigin(origin, 100, 0);
            // Assign data from DB to local variables
            var times = new List<string>();
            var values = new List<int>();
            for (int i = 0; i < dataList.Count; i++)
            {
                times.Add(dataList[i].Time.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add(dataList[i].Saldo);
            }
            times.Reverse();
            values.Reverse();
            var model = new DashboardViewModel
            {
                Times = times,
                Values = values,
                Origin = origin
            };
            return model;
        }

        public IActionResult UpdateChart(string origin)
        {
            var data = GetData(origin);
            data.Origins = m_DataStore.GetOrigins().ToList();
            return Json(data);
        }

        public IActionResult GetLatestData(string origin)
        {
            Console.WriteLine($"LatestData origin: {origin}");
            var data = GetData(origin);
            data.Origins = m_DataStore.GetOrigins().ToList();
            Console.WriteLine($"LastValue: {data.Values[data.Values.Count - 1]}");
            return Json(data);
        }
    }

}