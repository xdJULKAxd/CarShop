using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarShop.Models.ViewModels
{
    public class CarModelVM
    {
        public CarModel CarModel { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> BrandList{ get; set; }
    }
}
