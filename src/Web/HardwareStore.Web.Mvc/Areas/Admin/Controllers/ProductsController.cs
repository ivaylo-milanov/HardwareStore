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
        private static readonly JsonSerializerOptions AssemblyEditorJson = new()
        {
            PropertyNamingPolicy = null,
        };

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
            return this.View(new ProductFormModel
            {
                AssemblyComponents = CreateEmptyAssemblyRows(),
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductFormModel form)
        {
            this.NormalizeManufacturerId(form);
            this.ValidateOptionsJson(form);
            await this.ValidateAssemblyComponentsAsync(form, assemblyProductId: null).ConfigureAwait(false);
            if (!this.ModelState.IsValid)
            {
                EnsureAssemblyRowPadding(form);
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
            await this.PersistAssemblyComponentsAsync(product.Id, form).ConfigureAwait(false);
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
            var editModel = new ProductFormModel
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
            };
            await this.LoadAssemblyComponentsIntoFormAsync(editModel, product.Id).ConfigureAwait(false);
            return this.View(editModel);
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
            await this.ValidateAssemblyComponentsAsync(form, form.Id).ConfigureAwait(false);
            if (!this.ModelState.IsValid)
            {
                EnsureAssemblyRowPadding(form);
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

            await this.PersistAssemblyComponentsAsync(product.Id, form).ConfigureAwait(false);
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
            var usedInAssembly = await this.repository.AnyAsync<ProductAssemblyComponent>(a => a.ComponentProductId == id);
            if (inOrder || inCart || inFav || usedInAssembly)
            {
                this.TempData["ErrorMessage"] =
                    "Cannot delete a product that appears in orders, shopping carts, favorites, or another product's assembly.";
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

            var productRows = await this.repository.AllReadonly<Product>()
                .OrderBy(p => p.Name)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.ReferenceNumber,
                    CategoryName = p.Category.Name,
                })
                .ToListAsync();

            var catalog = productRows
                .Select(p => new { id = p.Id, label = $"{p.Name} ({p.ReferenceNumber})", category = p.CategoryName })
                .ToList();
            this.ViewBag.AssemblyProductCatalogJson = JsonSerializer.Serialize(catalog, AssemblyEditorJson);

            var roleFilters = new Dictionary<string, string[]>();
            foreach (AssemblyRoleKind k in Enum.GetValues<AssemblyRoleKind>())
            {
                if (k is AssemblyRoleKind.None or AssemblyRoleKind.Custom)
                {
                    continue;
                }

                var names = AssemblyRoleMapping.CategoryNamesForFilter(k);
                if (names.Count > 0)
                {
                    roleFilters[((int)k).ToString()] = names.ToArray();
                }
            }

            this.ViewBag.AssemblyRoleCategoryFiltersJson = JsonSerializer.Serialize(roleFilters, AssemblyEditorJson);

            this.ViewBag.AssemblyRoleKindList = new SelectList(
                new[]
                {
                    new { Value = (int)AssemblyRoleKind.None, Text = "— None —" },
                    new { Value = (int)AssemblyRoleKind.Cpu, Text = "CPU" },
                    new { Value = (int)AssemblyRoleKind.Gpu, Text = "GPU" },
                    new { Value = (int)AssemblyRoleKind.Ram, Text = "RAM" },
                    new { Value = (int)AssemblyRoleKind.Psu, Text = "PSU" },
                    new { Value = (int)AssemblyRoleKind.Motherboard, Text = "Motherboard" },
                    new { Value = (int)AssemblyRoleKind.Case, Text = "Case" },
                    new { Value = (int)AssemblyRoleKind.InternalDrives, Text = "Internal drives (HDD/SSD)" },
                    new { Value = (int)AssemblyRoleKind.Cooler, Text = "Cooler" },
                    new { Value = (int)AssemblyRoleKind.Custom, Text = "Custom…" },
                },
                "Value",
                "Text");
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

        private static List<AssemblyComponentInputModel> CreateEmptyAssemblyRows() =>
            new List<AssemblyComponentInputModel>
            {
                new() { RoleKind = AssemblyRoleKind.Cpu },
                new() { RoleKind = AssemblyRoleKind.Gpu },
                new() { RoleKind = AssemblyRoleKind.Ram },
                new() { RoleKind = AssemblyRoleKind.Psu },
                new() { RoleKind = AssemblyRoleKind.Motherboard },
                new() { RoleKind = AssemblyRoleKind.Case },
                new() { RoleKind = AssemblyRoleKind.InternalDrives },
                new() { RoleKind = AssemblyRoleKind.Cooler },
            };

        private static void EnsureAssemblyRowPadding(ProductFormModel form)
        {
            form.AssemblyComponents ??= new List<AssemblyComponentInputModel>();
            if (form.AssemblyComponents.Count == 0)
            {
                form.AssemblyComponents.AddRange(CreateEmptyAssemblyRows());
            }
        }

        private async Task LoadAssemblyComponentsIntoFormAsync(ProductFormModel form, int productId)
        {
            var lines = await this.repository.AllReadonly<ProductAssemblyComponent>()
                .Where(a => a.AssemblyProductId == productId)
                .OrderBy(a => a.SortOrder)
                .ThenBy(a => a.Id)
                .ToListAsync()
                .ConfigureAwait(false);

            if (lines.Count == 0)
            {
                form.AssemblyComponents = CreateEmptyAssemblyRows();
                return;
            }

            form.AssemblyComponents = lines.Select(a =>
            {
                var kind = AssemblyRoleMapping.FromStoredRole(a.Role);
                return new AssemblyComponentInputModel
                {
                    RoleKind = kind,
                    CustomRole = kind == AssemblyRoleKind.Custom ? a.Role : null,
                    ComponentProductId = a.ComponentProductId,
                    Quantity = a.Quantity,
                };
            }).ToList();
        }

        private async Task ValidateAssemblyComponentsAsync(ProductFormModel form, int? assemblyProductId)
        {
            var rows = form.AssemblyComponents ?? new List<AssemblyComponentInputModel>();
            for (var i = 0; i < rows.Count; i++)
            {
                var r = rows[i];
                var persistedRole = AssemblyRoleMapping.ToPersistedRole(r.RoleKind, r.CustomRole);
                var hasPid = r.ComponentProductId > 0;

                if (!hasPid && r.RoleKind == AssemblyRoleKind.Custom && !string.IsNullOrWhiteSpace(r.CustomRole))
                {
                    this.ModelState.AddModelError(
                        $"AssemblyComponents[{i}].ComponentProductId",
                        "Select a component product for this custom part.");
                    continue;
                }

                if (!hasPid)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(persistedRole))
                {
                    this.ModelState.AddModelError(
                        $"AssemblyComponents[{i}].RoleKind",
                        "Choose a part type, or pick Custom… and enter a label.");
                    continue;
                }

                if (r.RoleKind == AssemblyRoleKind.Custom && string.IsNullOrWhiteSpace(r.CustomRole))
                {
                    this.ModelState.AddModelError(
                        $"AssemblyComponents[{i}].CustomRole",
                        "Enter a label for the custom part.");
                    continue;
                }

                if (r.Quantity < 1)
                {
                    this.ModelState.AddModelError(
                        $"AssemblyComponents[{i}].Quantity",
                        "Quantity must be at least 1.");
                }

                if (assemblyProductId.HasValue && r.ComponentProductId == assemblyProductId.Value)
                {
                    this.ModelState.AddModelError(
                        $"AssemblyComponents[{i}].ComponentProductId",
                        "A product cannot include itself as a component.");
                }

                if (!await this.repository.AnyAsync<Product>(p => p.Id == r.ComponentProductId).ConfigureAwait(false))
                {
                    this.ModelState.AddModelError(
                        $"AssemblyComponents[{i}].ComponentProductId",
                        "Unknown product.");
                    continue;
                }

                if (r.RoleKind is not AssemblyRoleKind.None and not AssemblyRoleKind.Custom)
                {
                    var categoryName = await this.repository.AllReadonly<Product>()
                        .Where(p => p.Id == r.ComponentProductId)
                        .Select(p => p.Category.Name)
                        .FirstOrDefaultAsync()
                        .ConfigureAwait(false);

                    if (!AssemblyRoleMapping.ProductCategoryMatchesRole(categoryName, r.RoleKind))
                    {
                        var expected = string.Join(", ", AssemblyRoleMapping.CategoryNamesForFilter(r.RoleKind));
                        this.ModelState.AddModelError(
                            $"AssemblyComponents[{i}].ComponentProductId",
                            $"The product's category must match this part type. Use a category such as: {expected}.");
                    }
                }
            }
        }

        private async Task PersistAssemblyComponentsAsync(int assemblyProductId, ProductFormModel form)
        {
            var existing = await this.repository.All<ProductAssemblyComponent>()
                .Where(a => a.AssemblyProductId == assemblyProductId)
                .ToListAsync()
                .ConfigureAwait(false);
            if (existing.Count > 0)
            {
                this.repository.RemoveRange(existing);
            }

            var rows = form.AssemblyComponents ?? new List<AssemblyComponentInputModel>();
            var effective = rows
                .Select(r => new
                {
                    Row = r,
                    Role = AssemblyRoleMapping.ToPersistedRole(r.RoleKind, r.CustomRole),
                })
                .Where(x => x.Row.ComponentProductId > 0 && !string.IsNullOrWhiteSpace(x.Role))
                .ToList();

            for (var i = 0; i < effective.Count; i++)
            {
                var x = effective[i];
                await this.repository.AddAsync(
                    new ProductAssemblyComponent
                    {
                        AssemblyProductId = assemblyProductId,
                        ComponentProductId = x.Row.ComponentProductId,
                        Role = x.Role!,
                        Quantity = x.Row.Quantity,
                        SortOrder = i,
                    }).ConfigureAwait(false);
            }
        }
    }
}
