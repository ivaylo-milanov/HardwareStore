namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Order;
    using HardwareStore.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;
        private readonly ILogger<OrderController> logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            this.orderService = orderService;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            OrderFormModel model;
            try
            {
                model = await this.orderService.GetOrderModel(HttpContext.User.GetUserId());
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
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
                await this.orderService.OrderAsync(model, HttpContext.User.GetUserId());
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return RedirectToAction("Success");
        }

        public IActionResult Success() => View();
    }
}
