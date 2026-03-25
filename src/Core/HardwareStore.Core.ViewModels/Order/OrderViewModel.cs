namespace HardwareStore.Core.ViewModels.Order
{
    using HardwareStore.Infrastructure.Models.Enums;

    public class OrderViewModel
    {
        public Guid OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public OrderStatus Status { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
