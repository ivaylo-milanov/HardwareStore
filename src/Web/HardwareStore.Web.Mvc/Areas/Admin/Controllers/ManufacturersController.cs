namespace HardwareStore.Web.Mvc.Areas.Admin.Controllers
{
    using HardwareStore.Core.ViewModels.Admin;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class ManufacturersController : AdminControllerBase
    {
        private readonly IRepository repository;

        public ManufacturersController(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var items = await this.repository
                .AllReadonly<Manufacturer>()
                .OrderBy(m => m.Name)
                .ToListAsync();

            return this.View(items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return this.View(new ManufacturerFormModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ManufacturerFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.repository.AddAsync(new Manufacturer { Name = model.Name.Trim() });
            await this.repository.SaveChangesAsync();
            this.TempData["StatusMessage"] = "Manufacturer created.";
            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await this.repository.All<Manufacturer>().FirstOrDefaultAsync(m => m.Id == id);
            if (entity == null)
            {
                return this.NotFound();
            }

            return this.View(new ManufacturerFormModel { Id = entity.Id, Name = entity.Name });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ManufacturerFormModel model)
        {
            if (!model.Id.HasValue)
            {
                return this.BadRequest();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var entity = await this.repository.All<Manufacturer>().FirstOrDefaultAsync(m => m.Id == model.Id.Value);
            if (entity == null)
            {
                return this.NotFound();
            }

            entity.Name = model.Name.Trim();
            await this.repository.SaveChangesAsync();
            this.TempData["StatusMessage"] = "Manufacturer updated.";
            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var hasProducts = await this.repository.AnyAsync<Product>(p => p.ManufacturerId == id);
            if (hasProducts)
            {
                this.TempData["ErrorMessage"] = "Cannot delete a manufacturer that still has products.";
                return this.RedirectToAction(nameof(this.Index));
            }

            var entity = await this.repository.All<Manufacturer>().FirstOrDefaultAsync(m => m.Id == id);
            if (entity == null)
            {
                return this.NotFound();
            }

            this.repository.Remove(entity);
            await this.repository.SaveChangesAsync();
            this.TempData["StatusMessage"] = "Manufacturer deleted.";
            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
