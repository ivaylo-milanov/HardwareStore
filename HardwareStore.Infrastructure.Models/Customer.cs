namespace HardwareStore.Infrastructure.Models
{
    using HardwareStore.Common;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;

    [Comment("customer table")]
    public class Customer : IdentityUser
    {
        public Customer()
        {
            this.Orders = new HashSet<ProductOrder>();
            this.ShoppingCartItems = new HashSet<ShoppingCartItem>();
            this.Favorites = new HashSet<Favorite>();
        }

        [Comment("customer first name")]
        [Required]
        [MaxLength(GlobalConstants.CustomerFirstNameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Comment("customer last name")]
        [Required]
        [MaxLength(GlobalConstants.CustomerLastNameMaxLength)]
        public string LastName { get; set; } = null!;

        [Comment("customer phone")]
        [Required]
        [MaxLength(GlobalConstants.CustomerPhoneMaxLength)]
        public string Phone { get; set; } = null!;

        [Comment("customer city")]
        [Required]
        [MaxLength(GlobalConstants.CustomerCityMaxLength)]
        public string City { get; set; } = null!;

        [Comment("customer area")]
        [Required]
        [MaxLength(GlobalConstants.CustomerAreaMaxLength)]
        public string Area { get; set; } = null!;

        [Comment("customer address")]
        [Required]
        [MaxLength(GlobalConstants.CustomerAddressMaxLength)]
        public string Address { get; set; } = null!; 
 
        public ICollection<ProductOrder> Orders { get; set; } = null!;

        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; } = null!;

        public ICollection<Favorite> Favorites { get; set; } = null!;
    }
}