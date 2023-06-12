namespace HardwareStore.Controllers
{
    using HardwareStore.Infrastructure.Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}