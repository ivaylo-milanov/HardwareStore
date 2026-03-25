namespace HardwareStore.Web.Mvc;

using HardwareStore.Core.ViewModels.Product;

/// <summary>
/// Resolves nav/json category keys (e.g. from <c>categories-nav.json</c>) to product view model and filter types.
/// </summary>
public static class ProductCategoryTypes
{
    private const string ViewModelsAssembly = "HardwareStore.Core.ViewModels";

    /// <summary>
    /// Maps the category segment used in URLs to the namespace segment used by types (RAM → Ram).
    /// </summary>
    public static string ToTypeNamespace(string category) =>
        string.Equals(category, "RAM", StringComparison.OrdinalIgnoreCase) ? "Ram" : category;

    public static bool TryResolve(string? category, out Type viewModelType, out Type filterOptionsType)
    {
        viewModelType = null!;
        filterOptionsType = null!;
        if (string.IsNullOrWhiteSpace(category))
            return false;

        var ns = ToTypeNamespace(category.Trim());
        var vmName = $"HardwareStore.Core.ViewModels.{ns}.{ns}ViewModel, {ViewModelsAssembly}";
        var filterName = $"HardwareStore.Core.ViewModels.{ns}.{ns}FilterOptions, {ViewModelsAssembly}";

        var vm = Type.GetType(vmName, throwOnError: false, ignoreCase: true);
        var fo = Type.GetType(filterName, throwOnError: false, ignoreCase: true);
        if (vm == null || fo == null)
            return false;

        if (!typeof(ProductViewModel).IsAssignableFrom(vm) || !typeof(ProductFilterOptions).IsAssignableFrom(fo))
            return false;

        viewModelType = vm;
        filterOptionsType = fo;
        return true;
    }
}
