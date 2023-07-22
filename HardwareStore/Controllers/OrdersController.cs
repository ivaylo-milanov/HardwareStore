namespace HardwareStore.Controllers
{
    using HardwareStore.Infrastructure.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class OrdersController : Controller
    {
        private readonly UserManager<Customer> userManager;

        public OrdersController(UserManager<Customer> userManager)
        {
            userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var orders = user.Orders.ToList();

            return View(orders);
        }
    }
}
