namespace HardwareStore.Components
{
    using HardwareStore.Core.ViewModels.Product;
    using Microsoft.AspNetCore.Mvc;

    public class ProductsComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<ProductViewModel> models) => View(models);
    }
}
