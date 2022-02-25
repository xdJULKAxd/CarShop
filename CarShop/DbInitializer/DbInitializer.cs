using CarShop.Data;
using CarShop.Models;
using CarShop.StaticData;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarShop.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public DbInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public void Initialize()
        {
            
            if (_context.Database.GetPendingMigrations().Count() > 0)
            {
                _context.Database.Migrate();
            }

            if (!_roleManager.RoleExistsAsync(Role.Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(Role.Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Role.Client)).GetAwaiter().GetResult();

            }

            var adminUser = _context.AppUsers.FirstOrDefault(user => user.Name == "Admin");
           
            if (adminUser == null)
            {
                _userManager.CreateAsync(
                    new AppUser
                    {
                        Name = "Admin",
                        UserName = "admin@carshop.pl",
                        Email = "admin@carshop.pl",
                        PhoneNumber = "123123123"
                    }, "H4PiC*N325")
                    .GetAwaiter()
                    .GetResult();

                adminUser = _context.AppUsers.FirstOrDefault(user => user.Name == "Admin");

                if (adminUser != null)
                {
                    _userManager.AddToRoleAsync(adminUser, Role.Admin).GetAwaiter().GetResult();
                }
            }
        }
    }
}
