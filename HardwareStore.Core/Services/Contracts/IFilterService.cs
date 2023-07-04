namespace HardwareStore.Core.Services.Contracts
{
    using HardwareStore.Core.ViewModels.Product;

    public interface IFilterService
    {
        IEnumerable<TModel> FilterProducts<TModel, TFilter>(IEnumerable<TModel> products, TFilter filter)
            where TFilter : ProductFilterOptions
            where TModel : ProductViewModel;

        IEnumerable<T> OrderProducts<T>(IEnumerable<T> products, string order) where T : ProductViewModel;
    }
}
