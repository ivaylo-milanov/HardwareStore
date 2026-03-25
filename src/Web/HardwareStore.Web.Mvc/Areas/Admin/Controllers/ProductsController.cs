namespace HardwareStore.Web.Mvc.Areas.Admin.Controllers
{
    using System.Text.Json;
    using HardwareStore.Core.ViewModels.Admin;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class ProductsController : AdminControllerBase
    {
        private readonly IRepository repository;

        public ProductsController(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var items = await this.repository
                .AllReadonly<Product>()
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .OrderBy(p => p.Name)
                .Select(p => new ProductListItemViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    CategoryName = p.Category.Name,
                    ManufacturerName = p.Manufacturer != null ? p.Manufacturer.Name : null,
                    Price = p.Price,
                    Quantity = p.Quantity,
                })
                .ToListAsync();

            return this.View(items);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await this.FillSelectListsAsync();
            return this.View(new ProductFormModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductFormModel form)
        {
            this.NormalizeManufacturerId(form);
            this.ValidateOptionsJson(form);
            if (!this.ModelState.IsValid)
            {
                await this.FillSelectListsAsync();
                return this.View(form);
            }

            var options = string.IsNullOrWhiteSpace(form.Options) ? "{}" : form.Options.Trim();
            var product = new Product
            {
                Name = form.Name.Trim(),
                Price = form.Price,
                Quantity = form.Quantity,
                Description = string.IsNullOrWhiteSpace(form.Description) ? null : form.Description.Trim(),
                Warranty = form.Warranty,
                ManufacturerId = form.ManufacturerId,
                Model = string.IsNullOrWhiteSpace(form.Model) ? null : form.Model.Trim(),
                ReferenceNumber = form.ReferenceNumber.Trim(),
                CategoryId = form.CategoryId,
                Options = options,
                AddDate = DateTime.UtcNow,
            };

            await this.repository.AddAsync(product);
            await this.repository.SaveChangesAsync();
            this.TempData["StatusMessage"] = "Product created.";
            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await this.repository.All<Product>().FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return this.NotFound();
            }

            await this.FillSelectListsAsync();
            return this.View(new ProductFormModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                Description = product.Description,
                Warranty = product.Warranty,
                ManufacturerId = product.ManufacturerId,
                Model = product.Model,
                ReferenceNumber = product.ReferenceNumber,
                CategoryId = product.CategoryId,
                Options = string.IsNullOrWhiteSpace(product.Options) ? "{}" : product.Options,
                AddDate = product.AddDate,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductFormModel form)
        {
            if (!form.Id.HasValue)
            {
                return this.BadRequest();
            }

            this.NormalizeManufacturerId(form);
            this.ValidateOptionsJson(form);
            if (!this.ModelState.IsValid)
            {
                await this.FillSelectListsAsync();
                return this.View(form);
            }

            var product = await this.repository.All<Product>().FirstOrDefaultAsync(p => p.Id == form.Id.Value);
            if (product == null)
            {
                return this.NotFound();
            }

            var options = string.IsNullOrWhiteSpace(form.Options) ? "{}" : form.Options.Trim();
            product.Name = form.Name.Trim();
            product.Price = form.Price;
            product.Quantity = form.Quantity;
            product.Description = string.IsNullOrWhiteSpace(form.Description) ? null : form.Description.Trim();
            product.Warranty = form.Warranty;
            product.ManufacturerId = form.ManufacturerId;
            product.Model = string.IsNullOrWhiteSpace(form.Model) ? null : form.Model.Trim();
            product.ReferenceNumber = form.ReferenceNumber.Trim();
            product.CategoryId = form.CategoryId;
            product.Options = options;
            if (form.AddDate.HasValue)
            {
                product.AddDate = form.AddDate.Value;
            }

            await this.repository.SaveChangesAsync();
            this.TempData["StatusMessage"] = "Product updated.";
            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var inOrder = await this.repository.AnyAsync<ProductOrder>(po => po.ProductId == id);
            var inCart = await this.repository.AnyAsync<ShoppingCartItem>(ci => ci.ProductId == id);
            var inFav = await this.repository.AnyAsync<Favorite>(f => f.ProductId == id);
            if (inOrder || inCart || inFav)
            {
                this.TempData["ErrorMessage"] =
                    "Cannot delete a product that appears in orders, shopping carts, or favorites.";
                return this.RedirectToAction(nameof(this.Index));
            }

            var product = await this.repository.All<Product>().FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return this.NotFound();
            }

            this.repository.Remove(product);
            await this.repository.SaveChangesAsync();
            this.TempData["StatusMessage"] = "Product deleted.";
            return this.RedirectToAction(nameof(this.Index));
        }

        private async Task FillSelectListsAsync()
        {
            var categories = await this.repository.AllReadonly<Category>().OrderBy(c => c.Name).ToListAsync();
            var manufacturers = await this.repository.AllReadonly<Manufacturer>().OrderBy(m => m.Name).ToListAsync();
            this.ViewBag.CategoryList = new SelectList(categories, "Id", "Name");
            this.ViewBag.ManufacturerList = new SelectList(manufacturers, "Id", "Name");
        }

        private void NormalizeManufacturerId(ProductFormModel form)
        {
            if (form.ManufacturerId == 0)
            {
                form.ManufacturerId = null;
            }
        }

        private void ValidateOptionsJson(ProductFormModel form)
        {
            var raw = string.IsNullOrWhiteSpace(form.Options) ? "{}" : form.Options.Trim();
            try
            {
                using var doc = JsonDocument.Parse(raw);
                if (doc.RootElement.ValueKind != JsonValueKind.Object)
                {
                    this.ModelState.AddModelError(
                        nameof(form.Options),
                        "Options must be a JSON object (e.g. {\"Key\":\"Value\"}).");
                }
            }
            catch (JsonException)
            {
                this.ModelState.AddModelError(nameof(form.Options), "Options must be valid JSON.");
            }
        }
    }
}
