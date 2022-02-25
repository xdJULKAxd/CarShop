using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CarShop.Models
{
    public class Brand
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [ValidateNever]

        public IEnumerable<CarModel> CarModels { get; set; }
    }
}
