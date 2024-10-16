namespace HardwareStore.Core.Services.Contracts
{
    using HardwareStore.Core.ViewModels.Profile;

    public interface IProfileService
    {
        Task<ProfileViewModel> GetProfileModel(string userId);
    }
}
