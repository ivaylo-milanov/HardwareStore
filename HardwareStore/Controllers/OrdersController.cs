namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Order;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<OrderViewModel> orders;
            try
            {
                orders = await this.orderService.GetUserOrders(GetUserId());
            }
            catch (Exception)
            {
                throw;
            }

            return View(orders);
        }

        private string GetUserId() => HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
