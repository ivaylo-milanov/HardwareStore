namespace HardwareStore.Core.Services.Contracts
{
    using HardwareStore.Core.ViewModels.Home;

    public interface IHomeService
    {
        Task<IEnumerable<NewProductViewModel>> GetNewProducts();


    }
}
