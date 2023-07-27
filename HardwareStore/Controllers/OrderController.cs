namespace HardwareStore.Controllers
{
    using HardwareStore.Core.ViewModels.Order;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            OrderFormModel model = new OrderFormModel();

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(OrderFormModel model)
        {
            return View();
        }
    }
}
