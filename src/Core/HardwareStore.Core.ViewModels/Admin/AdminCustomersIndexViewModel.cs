namespace HardwareStore.Core.ViewModels.Admin
{
    public class AdminCustomersIndexViewModel
    {
        public IReadOnlyList<CustomerListItemViewModel> Items { get; set; } = Array.Empty<CustomerListItemViewModel>();

        public int Page { get; set; } = 1;

        public int TotalPages { get; set; }

        public int TotalCount { get; set; }
    }
}
