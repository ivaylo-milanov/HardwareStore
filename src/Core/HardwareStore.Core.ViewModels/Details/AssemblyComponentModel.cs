namespace HardwareStore.Core.ViewModels.Details
{
    public class AssemblyComponentModel
    {
        public string Role { get; set; } = null!;

        public int ProductId { get; set; }

        public string Name { get; set; } = null!;

        public string ReferenceNumber { get; set; } = null!;

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
