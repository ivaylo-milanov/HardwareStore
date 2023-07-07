namespace HardwareStore.Core.Services.Contracts
{
    using HardwareStore.Core.ViewModels.Search;

    public interface ISearchService
    {
        Task<IEnumerable<SearchViewModel>> GetProductsByKeyword(string keyword); 
    }
}
