namespace HardwareStore.Web.Api.Models
{
    using System.ComponentModel.DataAnnotations;
    using HardwareStore.Common;

    public class AuthTokenResponse
    {
        public string AccessToken { get; set; } = null!;

        public int ExpiresIn { get; set; }

        public string Email { get; set; } = null!;
    }

    public class CatalogFilterRequest
    {
        public string FilterJson { get; set; } = "{}";

        public int Order { get; set; } = 1;
    }

    public class SearchFilterRequest
    {
        public string Keyword { get; set; } = string.Empty;

        public string FilterJson { get; set; } = "{}";

        public int Order { get; set; } = 1;
    }

    public class AddCartItemRequest
    {
        [Required]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;
    }

    public class AddFavoriteRequest
    {
        [Required]
        public int ProductId { get; set; }
    }

    public class ProfileUpdateRequest
    {
        [Required]
        [MaxLength(GlobalConstants.CustomerFirstNameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(GlobalConstants.CustomerLastNameMaxLength)]
        public string LastName { get; set; } = null!;

        [Required]
        public string Phone { get; set; } = null!;

        [Required]
        [MaxLength(GlobalConstants.CustomerCityMaxLength)]
        public string City { get; set; } = null!;

        [Required]
        [MaxLength(GlobalConstants.CustomerAreaMaxLength)]
        public string Area { get; set; } = null!;

        [Required]
        [MaxLength(GlobalConstants.CustomerAddressMaxLength)]
        public string Address { get; set; } = null!;
    }
}
