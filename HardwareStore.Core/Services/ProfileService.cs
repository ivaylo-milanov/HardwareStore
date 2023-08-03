namespace HardwareStore.Core.Services
{
    using HardwareStore.Common;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Profile;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using System.Threading.Tasks;

    public class ProfileService : IProfileService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<Customer> userManager;

        public ProfileService(IHttpContextAccessor httpContextAccessor, UserManager<Customer> userManager)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }

        public async Task<ProfileViewModel> GetProfileModel()
        {
            var userPrincipal = httpContextAccessor.HttpContext.User;

            var user = await userManager.GetUserAsync(userPrincipal);

            if (user == null)
            {
                throw new ArgumentNullException(ExceptionMessages.UserNotFound);
            }

            ProfileViewModel model = new ProfileViewModel
            {
                FullName = user.FirstName + ' ' + user.LastName,
                City = user.City,
                Address = user.Address,
                Email = user.Email
            };

            return model;
        }
    }
}
