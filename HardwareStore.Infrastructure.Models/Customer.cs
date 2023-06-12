namespace HardwareStore.Infrastructure.Models
{
    using Microsoft.AspNetCore.Identity;

    public class Customer : IdentityUser
    {
        public Customer()
        {
            this.Orders = new HashSet<ProductOrder>();
        }

        public virtual ICollection<ProductOrder> Orders { get; set; } = null!;
    }
}