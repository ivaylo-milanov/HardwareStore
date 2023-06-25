namespace HardwareStore.Core.Services.Contracts
{
    using HardwareStore.Core.ViewModels.Mouse;

    public interface IMouseService
    {
        Task<IEnumerable<MouseViewModel>> GetAllMouses();

        Task<MousesViewModel> GetModel();

        IEnumerable<MouseViewModel> GetFilteredMouses(IEnumerable<MouseViewModel> mouses, MouseFilterOptions filter);
    }
}
