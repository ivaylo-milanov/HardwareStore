namespace HardwareStore.Core.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class CategoryAttribute : Attribute
    {
        public string CategoryName { get; set; } = null!;

        public CategoryAttribute(string categoryName)
        {
            CategoryName = categoryName;
        }
    }
}
