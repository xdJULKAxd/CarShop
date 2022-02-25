using CarShop.Data;
using CarShop.Models;
using CarShop.StaticData;
using CarShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CarShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Role.Admin)]
    public class CarController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CarController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult Index()
        {
            var cars = _context.Cars;
            return View(cars);
        }

        public IActionResult Upsert(int? id)
        {
            var carVM = new CarVM()
            {
                Car = new Car(),
                BrandList = _context.Brands.Select(brand => new SelectListItem
                {
                    Text = brand.Name,
                    Value = brand.Id.ToString()
                }).ToList()
            };

            if (id != null && id != 0)
            {
                carVM.Car = _context.Cars
                    .Include(x => x.CarModel)
                    .Include(x => x.CarModel.Brand)
                    .FirstOrDefault(x => x.Id == id);

                if (carVM.Car == null)
                {
                    return NotFound();
                }
            }
            return View(carVM);
        }

        [HttpPost]
        public IActionResult Upsert(CarVM obj, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            #region Adding Image to the object
            if (file != null)
            {
                var wwwRootPath = _environment.WebRootPath;
                var uploads = Path.Combine(wwwRootPath, @"images\cars");
                var extension = Path.GetExtension(file.FileName);
                var fileName = Guid.NewGuid().ToString();
                if (obj.Car.ImageUrl != null)
                {
                    var oldImagePath = Path.Combine(wwwRootPath, obj.Car.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                using var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create);
                file.CopyTo(fileStream);
                obj.Car.ImageUrl = $@"\images\cars\{fileName}{extension}";
            }
            #endregion

            obj.Car.VIN = obj.Car.VIN.ToUpper();
            obj.Car.Registration = obj.Car.Registration.ToUpper();
            obj.Car.CarModel = null;

            var carFromDb = _context.Cars.FirstOrDefault(x => x.Id == obj.Car.Id);
            if(carFromDb == null)
            {
                _context.Cars.Add(obj.Car);
                TempData["success"] = "Created a new Car";
            }
            else {
                _context.Cars.Update(obj.Car);
                TempData["success"] = "Updated a Car";
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        #region
        
        [HttpGet]
        public IActionResult GetAll()
        {
            var cars = _context.Cars
                .Include(x => x.CarModel)
                .Include(x => x.CarModel.Brand)
                .Select(x => new
                {
                    id = x.Id,
                    brand = x.CarModel.Brand.Name,
                    model = x.CarModel.Name,
                    price = $"${x.Price.ToString("0")}",
                    productionDate = x.ProductionDate.ToLocalTime().ToString("yyyy-MM-dd"),
                    registrationDate = x.RegistrationDate.ToLocalTime().ToString("yyyy-MM-dd"),
                    vin = x.VIN,
                    registration = x.Registration
                })
                .ToList();
            return Json(new { data = cars });
        }

        [HttpGet]
        public IActionResult GetAllCarModels()
        {
            var carModels = _context.CarModels.ToList();
            return Json(new { data = carModels });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false });
            }

            var obj = _context.Cars.FirstOrDefault(car => car.Id == id);

            if (obj == null)
            {
                return Json(new { success = false, message = "Error" });
            }

            _context.Cars.Remove(obj);
            _context.SaveChanges();

            TempData["success"] = "Removed a Car";

            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion
    }
}
