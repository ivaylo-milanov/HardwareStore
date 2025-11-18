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
            var customer = await this.repository.FindAsync<Customer>(userId);

            if (customer == null)
            {
                throw new ArgumentNullException(ExceptionMessages.UserNotFound);
            }

            ProfileViewModel model = new ProfileViewModel
            {
                FullName = customer.FirstName + ' ' + customer.LastName,
                City = customer.City,
                Address = customer.Address,
                Email = customer.Email
            };

            return model;
        }
    }
}
