namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Product;
    using Microsoft.AspNetCore.Mvc;

    public class ProductController : Controller
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        public async Task<IActionResult> Details(int id)
        {
            ProductDetailsModel model;
            try
            {
                model = await this.productService.GetProductDetails(id);
            }
            catch (ArgumentNullException)
            {
                throw;
            }

            return View(model);
        }
    }
}
