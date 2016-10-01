using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CyclingTrips.ViewModels;
using CyclingTrips.Models;
using CyclingTripsCore.Services;

namespace CyclingTrips.Controllers
{
    public class HomeController : Controller
    {
        private ICyclingTripsRepository _repository;

        public HomeController(ICyclingTripsRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
