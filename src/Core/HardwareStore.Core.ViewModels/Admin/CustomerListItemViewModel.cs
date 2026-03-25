namespace HardwareStore.Core.ViewModels.Admin
{
    public class CustomerListItemViewModel
    {
        public string Id { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public bool IsAdmin { get; set; }
    }
}
