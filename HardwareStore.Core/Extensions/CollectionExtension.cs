namespace HardwareStore.Core.Extensions
{
    using HardwareStore.Infrastructure.Models;

    public static class CollectionExtension
    {
        public static string GetAttributeValue(this ICollection<ProductAttribute> attributes, string key)
            => attributes.FirstOrDefault(a => a.Name == key)!.Value;
    }
}
