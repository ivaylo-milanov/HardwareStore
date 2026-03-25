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
    public class CheckoutController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly ILogger<CheckoutController> logger;

        public CheckoutController(IOrderService orderService, ILogger<CheckoutController> logger)
        {
            this.orderService = orderService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<OrderFormModel>> Get()
        {
            try
            {
                var model = await this.orderService.GetOrderModel(this.User.GetUserId());
                return this.Ok(model);
            }
            catch (InvalidOperationException ex)
            {
                this.logger.LogError(ex, LogMessages.CheckoutOperationFailed);
                return this.Problem(statusCode: StatusCodes.Status400BadRequest, detail: ex.Message);
            }
        }

        [HttpPost("place")]
        public async Task<IActionResult> Place([FromBody] OrderFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                try
                {
                    var totals = await this.orderService.GetOrderModel(this.User.GetUserId());
                    model.TotalAmount = totals.TotalAmount;
                }
                catch (InvalidOperationException ex)
                {
                    this.logger.LogError(ex, LogMessages.CheckoutOperationFailed);
                    return this.Problem(statusCode: StatusCodes.Status400BadRequest, detail: ex.Message);
                }

                return this.ValidationProblem(this.ModelState);
            }

            try
            {
                var preview = await this.orderService.GetOrderModel(this.User.GetUserId());
                model.TotalAmount = preview.TotalAmount;
                await this.orderService.OrderAsync(model, this.User.GetUserId());
            }
            catch (InvalidOperationException ex)
            {
                this.logger.LogError(ex, LogMessages.CheckoutOperationFailed);
                return this.Problem(statusCode: StatusCodes.Status400BadRequest, detail: ex.Message);
            }

            return this.NoContent();
        }
    }
}
