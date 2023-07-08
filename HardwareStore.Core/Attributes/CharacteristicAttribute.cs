namespace HardwareStore.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CharacteristicAttribute : Attribute
    {
        public CharacteristicAttribute()
        {
        }

        public CharacteristicAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
