namespace HardwareStore.Web.Mvc.Controllers
{
    using HardwareStore.Common;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Order;
    using HardwareStore.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class CheckoutController : Controller
    {
        #region Fields and construction

        private readonly IOrderService orderService;
        private readonly ILogger<CheckoutController> logger;

        public CheckoutController(IOrderService orderService, ILogger<CheckoutController> logger)
        {
            this.orderService = orderService;
            this.logger = logger;
        }

        #endregion

        #region Checkout flow

        public async Task<IActionResult> Index()
        {
            try
            {
                var model = await this.orderService.GetOrderModel(this.User.GetUserId());
                return this.View(model);
            }
            catch (InvalidOperationException ex)
            {
                this.logger.LogError(ex, LogMessages.CheckoutOperationFailed);
                return this.RedirectToAction("Error", "Home", new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Place(
            [Bind(
                nameof(OrderFormModel.FirstName),
                nameof(OrderFormModel.LastName),
                nameof(OrderFormModel.Phone),
                nameof(OrderFormModel.City),
                nameof(OrderFormModel.Area),
                nameof(OrderFormModel.Address),
                nameof(OrderFormModel.AdditionalNotes),
                nameof(OrderFormModel.PaymentMethod))]
            OrderFormModel model)
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
                    return this.RedirectToAction("Error", "Home", new { message = ex.Message });
                }

                return this.View(nameof(Index), model);
            }

            try
            {
                await this.orderService.OrderAsync(model, this.User.GetUserId());
            }
            catch (InvalidOperationException ex)
            {
                this.logger.LogError(ex, LogMessages.CheckoutOperationFailed);
                return this.RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return this.RedirectToAction(nameof(Success));
        }

        public IActionResult Success() => this.View();

        #endregion
    }
}
