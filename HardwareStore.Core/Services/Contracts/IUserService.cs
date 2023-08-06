namespace HardwareStore.Core.Services.Contracts
{
    using HardwareStore.Core.ViewModels.Favorite;
    using HardwareStore.Core.ViewModels.ShoppingCart;
    using HardwareStore.Core.ViewModels.User;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.AspNetCore.Identity;

    public interface IUserService
    {
        Task<SignInResult> LoginAsync(LoginFormModel model, ICollection<ShoppingCartExportModel> cart, ICollection<int> favorites);

        Task<IdentityResult> RegisterAsync(RegisterFormModel model, ICollection<ShoppingCartExportModel> cart, ICollection<int> favorites);

        Task LogoutAsync();

        Task<ICollection<Favorite>> GetCustomerFavorites(string userId);
    }
}
