namespace HardwareStore.Core.Services.Contracts
{
    using HardwareStore.Core.ViewModels.User;
    using Microsoft.AspNetCore.Identity;

    public interface IUserService
    {
        Task<SignInResult> LoginAsync(LoginFormModel model);

        Task<IdentityResult> RegisterAsync(RegisterFormModel model);

        Task LogoutAsync();
    }
}
