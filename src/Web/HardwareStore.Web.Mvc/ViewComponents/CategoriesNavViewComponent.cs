namespace HardwareStore.Web.Mvc.ViewComponents
{
    using HardwareStore.Core.ViewModels;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using HardwareStore.Infrastructure.Models.Enums;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class CategoriesNavViewComponent : ViewComponent
    {
        private readonly IRepository repository;

        public CategoriesNavViewComponent(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var peripherals = await this.repository.AllReadonly<Category>()
                .Where(c => c.Group == CategoryGroup.Peripherals)
                .OrderBy(c => c.Name)
                .Select(c => new CategoryNavLinkItem(c.Name, c.Name))
                .ToListAsync()
                .ConfigureAwait(false);

            var hardware = await this.repository.AllReadonly<Category>()
                .Where(c => c.Group == CategoryGroup.Hardware)
                .OrderBy(c => c.Name)
                .Select(c => new CategoryNavLinkItem(c.Name, c.Name))
                .ToListAsync()
                .ConfigureAwait(false);

            var groups = new List<CategoryNavGroupViewModel>(2);
            if (peripherals.Count > 0)
            {
                groups.Add(new CategoryNavGroupViewModel("Peripherals", peripherals));
            }

            if (hardware.Count > 0)
            {
                groups.Add(new CategoryNavGroupViewModel("Hardware", hardware));
            }

            return this.View(groups);
        }
    }
}
