using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarShop.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        [Required]
        public string AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        [ValidateNever]
        public AppUser AppUser { get; set; }
        [Required]
        public int CarId { get; set; }
        [ForeignKey("CarId")]
        [ValidateNever]
        public Car Car { get; set; }

        [Required]
        public string Address { get; set; }
        [Required]
        public string PostCode { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public double Price { get; set; }
    }
}
