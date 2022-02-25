using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarShop.Models
{
    public class CarModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DisplayName("Brand")]
        public int BrandId { get; set; }
        [ForeignKey("BrandId")]
        [ValidateNever]
        public Brand Brand { get; set; }
    }
}
