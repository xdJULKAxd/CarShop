using CarShop.Data;
using CarShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var users = _context.AppUsers;
            return View(users);
        }

        #region API
        [HttpDelete]
        public IActionResult Delete(string? id)
        {
            if (id == null)
            {
                return Json(new { success = false });
            }

            var obj = _context.AppUsers.FirstOrDefault(urser => urser.Id == id);

            if (obj == null)
            {
                return Json(new { success = false, message = "Error" });
            }

            _context.AppUsers.Remove(obj);
            _context.SaveChanges();

            TempData["success"] = "Removed a User";

            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion
    }
}
