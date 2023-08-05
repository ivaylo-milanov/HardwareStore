namespace HardwareStore.Core.Services.Contracts
{
    using HardwareStore.Core.ViewModels.Favorite;
    using HardwareStore.Core.ViewModels.ShoppingCart;
    using HardwareStore.Core.ViewModels.User;
    using Microsoft.AspNetCore.Identity;

    public interface IUserService
    {
        Task<SignInResult> LoginAsync(LoginFormModel model, ICollection<ShoppingCartExportModel> cart, ICollection<int> favorites);

        Task<IdentityResult> RegisterAsync(RegisterFormModel model, ICollection<ShoppingCartExportModel> cart, ICollection<int> favorites);

        Task LogoutAsync();
    }
}
