namespace HardwareStore.Core.Services
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.User;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using System.Threading.Tasks;

    public class UserService : IUserService
    {
        private readonly SignInManager<Customer> signInManager;
        private readonly UserManager<Customer> userManager;

        public UserService(SignInManager<Customer> signInManager, UserManager<Customer> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public async Task<SignInResult> LoginAsync(LoginFormModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                throw new InvalidOperationException("The account does not exist.");
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
                Phone = model.Phone,
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
