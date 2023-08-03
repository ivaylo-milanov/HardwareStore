﻿namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.PowerSupply;
    using HardwareStore.Core.ViewModels.Product;
    using Microsoft.AspNetCore.Mvc;

    public class PowerSuppliesController : Controller
    {
        private readonly IProductService productService;

        public PowerSuppliesController(IProductService productService)
        {
            this.productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            ProductsViewModel<PowerSupplyViewModel> model;
            try
            {
                model = await this.productService.GetModel<PowerSupplyViewModel>();
            }
            catch (Exception)
            {
                throw;
            }

            return View(model);
        }

        public IActionResult FilterPowerSupplies([FromBody] PowerSupplyFilterOptions filter)
        {
            IEnumerable<PowerSupplyViewModel> filtered;
            try
            {
                filtered = this.productService.FilterProducts<PowerSupplyViewModel, PowerSupplyFilterOptions>(filter);
            }
            catch (ArgumentNullException)
            {
                throw;
            }

            return PartialView("_ProductsPartialView", filtered);
        }
    }
}
