namespace HardwareStore.Web.Mvc.Controllers
{
    using HardwareStore.Common;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Order;
    using HardwareStore.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class OrdersController : Controller
    {
        #region Fields and construction

        private readonly IOrderService orderService;
        private readonly ILogger<OrdersController> logger;

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            this.orderService = orderService;
            this.logger = logger;
        }

        #endregion

        #region Orders

        public async Task<IActionResult> Index()
        {
            IEnumerable<OrderViewModel> orders;
            try
            {
                orders = await this.orderService.GetUserOrders(User.GetUserId());
            }
            catch (InvalidOperationException ex)
            {
                this.logger.LogError(ex, LogMessages.OrdersLoadFailed);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return View(orders);
        }

        #endregion
    }
}
