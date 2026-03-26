namespace HardwareStore.Core.ViewModels
{
    /// <summary>One link in the Products dropdown (name and title from a category row).</summary>
    public record CategoryNavLinkItem(string Name, string Title);

    public record CategoryNavGroupViewModel(string Title, IReadOnlyList<CategoryNavLinkItem> Items);
}
