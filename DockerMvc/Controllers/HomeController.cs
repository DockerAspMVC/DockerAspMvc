using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DockerMvc.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DockerMvc.Models;

namespace DockerMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BaseDbContext _context;

        public HomeController(ILogger<HomeController> logger, BaseDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var profilesCount = _context.Profiles.Count();
            var productsCount = _context.Productos.Count();

            ViewBag.ProfilesCount = profilesCount;
            ViewBag.ProductsCount = productsCount;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}