using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CarShop.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string PostCode { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
    }
}
