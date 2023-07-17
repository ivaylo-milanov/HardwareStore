namespace HardwareStore.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ShoppingCardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
