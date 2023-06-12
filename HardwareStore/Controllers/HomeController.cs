namespace HardwareStore.Controllers
{
    using HardwareStore.Infrastructure.Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class HomeController : Controller
    {
        private readonly HardwareStoreDbContext context;

        public HomeController(HardwareStoreDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var something = await this.context
                .Products
                .Include(p => p.PartComputers)
                .Where(p => p.Id == 6)
                .ToListAsync();

            return View();
        }
    }
}