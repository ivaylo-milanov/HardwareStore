namespace HardwareStore.Components
{
    using HardwareStore.Core.ViewModels.Product;
    using Microsoft.AspNetCore.Mvc;

    public class FilterComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string name, IEnumerable<string> values) 
        {
            var filterValues = string.Join(", ", values)
                .Split(", ")
                .Select(v => v.Trim())
                .Distinct();

            FilterCategoryModel model = new FilterCategoryModel
            {
                Values = filterValues,
                Name = name
            };

            return View(model);
        }
    }
}
