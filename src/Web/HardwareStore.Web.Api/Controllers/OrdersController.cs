namespace HardwareStore.Web.Api.Controllers
{
    using HardwareStore.Web.Api.Extensions;
    using HardwareStore.Common;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Order;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly ILogger<OrdersController> logger;

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            this.orderService = orderService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderViewModel>>> Get()
        {
            try
            {
                var orders = await this.orderService.GetUserOrders(this.User.GetUserId());
                return this.Ok(orders);
            }
            catch (InvalidOperationException ex)
            {
                this.logger.LogError(ex, LogMessages.OrdersLoadFailed);
                return this.Problem(statusCode: StatusCodes.Status400BadRequest, detail: ex.Message);
            }
        }
    }
}
