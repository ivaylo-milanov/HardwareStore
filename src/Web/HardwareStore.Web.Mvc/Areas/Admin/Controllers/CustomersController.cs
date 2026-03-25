namespace HardwareStore.Web.Mvc.Areas.Admin.Controllers
{
    using HardwareStore.Core.ViewModels.Admin;
    using HardwareStore.Extensions;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class CustomersController : AdminControllerBase
    {
        private const int PageSize = 20;

        private readonly UserManager<Customer> userManager;

        public CustomersController(UserManager<Customer> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            if (page < 1)
            {
                page = 1;
            }

            var baseQuery = this.userManager.Users.OrderBy(u => u.Email);
            var totalCount = await baseQuery.CountAsync();
            var totalPages = totalCount == 0 ? 0 : (int)Math.Ceiling(totalCount / (double)PageSize);
            if (totalPages > 0 && page > totalPages)
            {
                page = totalPages;
            }

            var users = await baseQuery
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            var items = new List<CustomerListItemViewModel>();
            foreach (var u in users)
            {
                items.Add(new CustomerListItemViewModel
                {
                    Id = u.Id,
                    Email = u.Email ?? string.Empty,
                    FullName = $"{u.FirstName} {u.LastName}".Trim(),
                    IsAdmin = await this.userManager.IsInRoleAsync(u, AdminConstants.AdminRoleName),
                });
            }

            return this.View(new AdminCustomersIndexViewModel
            {
                Items = items,
                Page = page,
                TotalPages = totalPages,
                TotalCount = totalCount,
            });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);
            if (user == null)
            {
                return this.NotFound();
            }

            var model = new CustomerEditFormModel
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                City = user.City,
                Area = user.Area,
                Address = user.Address,
                IsAdmin = await this.userManager.IsInRoleAsync(user, AdminConstants.AdminRoleName),
            };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CustomerEditFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var user = await this.userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return this.NotFound();
            }

            user.FirstName = model.FirstName.Trim();
            user.LastName = model.LastName.Trim();
            user.PhoneNumber = model.PhoneNumber.Trim();
            user.City = model.City.Trim();
            user.Area = model.Area.Trim();
            user.Address = model.Address.Trim();

            var updateResult = await this.userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var err in updateResult.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, err.Description);
                }

                return this.View(model);
            }

            var isAdmin = await this.userManager.IsInRoleAsync(user, AdminConstants.AdminRoleName);
            if (model.IsAdmin && !isAdmin)
            {
                await this.userManager.AddToRoleAsync(user, AdminConstants.AdminRoleName);
            }
            else if (!model.IsAdmin && isAdmin)
            {
                await this.userManager.RemoveFromRoleAsync(user, AdminConstants.AdminRoleName);
            }

            this.TempData["StatusMessage"] = "Customer updated.";
            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
