using CarShop.Data;
using CarShop.Models;
using CarShop.Models.ViewModels;
using CarShop.StaticData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BrandShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Role.Admin)]
    public class CarModelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarModelController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var carModels = _context.CarModels;
            return View(carModels);
        }

        public IActionResult Upsert(int? id)
        {
            var carModel = new CarModelVM
            {
                CarModel = new CarModel(),
                BrandList = _context.Brands.Select(brand => new SelectListItem
                {
                    Text = brand.Name,
                    Value = brand.Id.ToString()
                })
            };

            if (id != null && id != 0)
            {
                carModel.CarModel = _context.CarModels.FirstOrDefault(brand => brand.Id == id);

                if (carModel.CarModel == null)
                {
                    return NotFound();
                }
            }
            return View(carModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CarModelVM obj)
        {
            if (!ModelState.IsValid || obj.CarModel?.BrandId == null)
            {
                return View();
            }

            if (obj.CarModel.Id == 0)
            {
                _context.CarModels.Add(obj.CarModel);
                TempData["success"] = "Created a new Car Model";
            }
            else
            {
                _context.CarModels.Update(obj.CarModel);
                TempData["success"] = "Updated a Car Model";
            }
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false });
            }

            var obj = _context.CarModels.FirstOrDefault(obj => obj.Id == id);

            if (obj == null)
            {
                return Json(new { success = false, message = "Error" });
            }

            _context.CarModels.Remove(obj);
            _context.SaveChanges();

            TempData["success"] = "Removed a Car Model";

            return Json(new { success = true, message = "Delete successful" });
        }
    }
}
