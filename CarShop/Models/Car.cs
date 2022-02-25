using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarShop.Models
{
    public class Car
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [DisplayName("Car Model")]
        public int CarModelId { get; set; }
        [ForeignKey("CarModelId")]
        [ValidateNever]
        public CarModel CarModel { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public double Price { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Production date")]
        public DateTime ProductionDate { get; set; } = DateTime.Now;
        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Registration date")]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        [Required]
        public DateTime PublicationDate { get; set; } = DateTime.Now;
        [Required]
        public string VIN { get; set; }
        [Required]
        public string Registration { get; set; }
        [Required]
        public double Milage { get; set; }
        [ValidateNever]
        public string? ImageUrl { get; set; }
    }
}
