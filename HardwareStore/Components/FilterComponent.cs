namespace HardwareStore.Components
{
    using HardwareStore.Core.Attributes;
    using HardwareStore.Core.ViewModels.Product;
    using Microsoft.AspNetCore.Mvc;
    using System.Reflection;

    public class FilterComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string name, IEnumerable<string> values) 
        {
            FilterCategoryModel model = new FilterCategoryModel
            {
                Values = values,
                Name = name
            };

            return View(model);
        }
    }
}
