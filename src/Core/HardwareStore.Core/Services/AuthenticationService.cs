namespace HardwareStore.Core.Services
{
    using HardwareStore.Common;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.User;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.AspNetCore.Identity;
    using System.Threading.Tasks;

    public class AuthenticationService : IAuthenticationService
    {
        private readonly SignInManager<Customer> signInManager;
        private readonly UserManager<Customer> userManager;

        public AuthenticationService(SignInManager<Customer> signInManager, UserManager<Customer> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public async Task<SignInResult> LoginAsync(LoginFormModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                throw new InvalidOperationException(ExceptionMessages.AccountNotFound);
            }

            var result = await signInManager.PasswordSignInAsync(user, model.Password, false, false);

            return result;
        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> RegisterAsync(RegisterFormModel model)
        {
            Customer user = new Customer
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.Phone,
                City = model.City,
                Area = model.Area,
                Address = model.Address
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
            }

            return result;
        }
    }
}
