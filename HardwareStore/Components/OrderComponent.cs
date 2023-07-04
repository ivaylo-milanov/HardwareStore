namespace HardwareStore.Components
{
    using HardwareStore.Core.ViewModels.Product;
    using Microsoft.AspNetCore.Mvc;

    public class OrderComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View();
    }
}
