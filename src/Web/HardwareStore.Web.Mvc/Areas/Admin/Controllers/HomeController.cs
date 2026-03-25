namespace HardwareStore.Web.Mvc.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : AdminControllerBase
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
