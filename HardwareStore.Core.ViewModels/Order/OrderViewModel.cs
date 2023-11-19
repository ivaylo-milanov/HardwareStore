namespace HardwareStore.Core.ViewModels.Order
{
    public class OrderViewModel
    {
        public string OrderId { get; set; }

        public string OrderDate { get; set; } = null!;

        public string Status { get; set; } = null!;

        public decimal TotalAmount { get; set; }
    }
}
