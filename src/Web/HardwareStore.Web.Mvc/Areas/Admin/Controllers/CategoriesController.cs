namespace HardwareStore.Web.Mvc.Areas.Admin.Controllers
{
    using HardwareStore.Core.ViewModels.Admin;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class CategoriesController : AdminControllerBase
    {
        private readonly IRepository repository;

        public CategoriesController(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var items = await this.repository
                .AllReadonly<Category>()
                .OrderBy(c => c.Name)
                .ToListAsync();

            return this.View(items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return this.View(new CategoryFormModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.repository.AddAsync(new Category { Name = model.Name.Trim(), Group = model.Group });
            await this.repository.SaveChangesAsync();
            this.TempData["StatusMessage"] = "Category created.";
            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await this.repository.All<Category>().FirstOrDefaultAsync(c => c.Id == id);
            if (entity == null)
            {
                return this.NotFound();
            }

            return this.View(new CategoryFormModel { Id = entity.Id, Name = entity.Name });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryFormModel model)
        {
            if (!model.Id.HasValue)
            {
                return this.BadRequest();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var entity = await this.repository.All<Category>().FirstOrDefaultAsync(c => c.Id == model.Id.Value);
            if (entity == null)
            {
                return this.NotFound();
            }

            entity.Name = model.Name.Trim();
            entity.Group = model.Group;
            await this.repository.SaveChangesAsync();
            this.TempData["StatusMessage"] = "Category updated.";
            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var hasProducts = await this.repository.AnyAsync<Product>(p => p.CategoryId == id);
            if (hasProducts)
            {
                this.TempData["ErrorMessage"] = "Cannot delete a category that still has products.";
                return this.RedirectToAction(nameof(this.Index));
            }

            var entity = await this.repository.All<Category>().FirstOrDefaultAsync(c => c.Id == id);
            if (entity == null)
            {
                return this.NotFound();
            }

            this.repository.Remove(entity);
            await this.repository.SaveChangesAsync();
            this.TempData["StatusMessage"] = "Category deleted.";
            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
