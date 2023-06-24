namespace HardwareStore.Core.Services.Contracts
{
    using HardwareStore.Core.ViewModels.Mouse;

    public interface IMouseService
    {
        Task<IEnumerable<MouseViewModel>> GetAllMouses();

        IEnumerable<MouseViewModel> GetFilteredMouses(IEnumerable<MouseViewModel> mouses, MouseFilterOptions filter);
    }
}
