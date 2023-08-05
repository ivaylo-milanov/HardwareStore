namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Order;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            OrderFormModel model;
            try
            {
                model = await this.orderService.GetOrderModel(GetUserId());
            }
            catch (Exception)
            {
                throw;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(OrderFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await this.orderService.OrderAsync(model, GetUserId());
            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToAction("Success");
        }

        public IActionResult Success() => View();

        private string GetUserId() => HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
