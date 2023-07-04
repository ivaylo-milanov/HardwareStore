namespace HardwareStore.Components
{
    using HardwareStore.Core.ViewModels.Product;
    using Microsoft.AspNetCore.Mvc;

    public class FilterComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string name, IEnumerable<string> values)
        {
            FilterCategoryModel model = new FilterCategoryModel
            {
                Name = name,
                Values = values
            };

            return View(model);
        }
    }
}
