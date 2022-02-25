using CarShop.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarShop.ViewModels
{
    public class CarVM
    {
        public Car Car { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> BrandList { get; set; }
    }
}
