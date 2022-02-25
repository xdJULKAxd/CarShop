using CarShop.Data;
using CarShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CarShop.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var cars = _context.Cars.Include("CarModel");
            return View(cars);
        }

        [Authorize]
        public IActionResult Details(int? id)
        {
            if (id == null || id == 0)
            {
                return View();
            }

            var car = _context.Cars
                .Include(x => x.CarModel)
                .Include(x => x.CarModel.Brand)
                .FirstOrDefault(x => x.Id == id);
            return View(car);
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