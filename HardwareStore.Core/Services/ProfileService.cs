namespace HardwareStore.Core.Services
{
    using HardwareStore.Common;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Profile;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using System.Threading.Tasks;

    public class ProfileService : IProfileService
    {
        private readonly IRepository repository;

        public ProfileService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<ProfileViewModel> GetProfileModel(string userId)
        {
            var user = await GetCustomer(userId);

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

        private async Task<Customer> GetCustomer(string userId)
            => await this.repository.FindAsync<Customer>(userId);
    }
}
