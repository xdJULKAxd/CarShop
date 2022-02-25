using CarShop.Data;
using CarShop.Models;
using CarShop.StaticData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = $"{Role.Admin}, {Role.Client}")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }


        [Authorize(Roles = Role.Admin)]
        public IActionResult Index()
        {
            var orders = _context.Orders
                .Include(x => x.Car)
                .Include(u => u.AppUser)
                .Include(y => y.Car.CarModel)
                .Include(z => z.Car.CarModel.Brand);

            return View(orders);
        }

        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                return View();
            }

            var car = _context.Cars.Include(x => x.CarModel).Include(x => x.CarModel.Brand).FirstOrDefault(c => c.Id == id);

            if (car == null)
            {
                return NotFound();
            }

            var appUser = _context.AppUsers.FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (appUser == null)
            {
                return NotFound();
            }

            var order = new Order
            {
                CarId = car.Id,
                Car = car,
                AppUserId = appUser.Id,
                AppUser = appUser,
                Price = car.Price,
                Address = appUser.Address,
                PostCode = appUser.PostCode,
                City = appUser.City,
                Country = appUser.Country
            };

            return View(order);
        }

        [HttpPost]
        public IActionResult Upsert(Order obj)
        {
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            obj.Id = 0;

            _context.Add(obj);
            _context.SaveChanges();

            return RedirectToAction("Summary", obj);
        }

        public IActionResult Summary(Order obj)
        {
            return View(obj);
        }
    }
}
