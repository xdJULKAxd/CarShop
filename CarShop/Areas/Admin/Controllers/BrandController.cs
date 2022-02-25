using CarShop.Data;
using CarShop.Models;
using CarShop.StaticData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrandShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Role.Admin)]
    public class BrandController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BrandController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var Brands = _context.Brands;
            return View(Brands);
        }

        public IActionResult Upsert(int? id)
        {
            var brand = new Brand();

            if (id != null && id != 0)
            {
                brand = _context.Brands.Include("CarModels").FirstOrDefault(brand => brand.Id == id);

                if (brand == null)
                {
                    return NotFound();
                }
            }
            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Brand obj)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (obj.Id == 0)
            {
                _context.Brands.Add(obj);
                TempData["success"] = "Created a new Brand";
            }
            else
            {
                _context.Brands.Update(obj);
                TempData["success"] = "Updated a Brand";
            }
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        #region 

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false });
            }

            var obj = _context.Brands.FirstOrDefault(brand => brand.Id == id);

            if (obj == null)
            {
                return Json(new { success = false, message = "Error" });
            }

            _context.Brands.Remove(obj);
            _context.SaveChanges();

            TempData["success"] = "Removed a Brand";

            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion
    }
}
