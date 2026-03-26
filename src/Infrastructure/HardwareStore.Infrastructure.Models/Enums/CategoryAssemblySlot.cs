namespace HardwareStore.Infrastructure.Models.Enums
{
    /// <summary>
    /// Standard PC assembly slot for a category. Numeric values match admin assembly role kinds for Cpu through Cooler (Custom is not used).
    /// </summary>
    public enum CategoryAssemblySlot
    {
        None = 0,

        Cpu = 1,

        Gpu = 2,

        Ram = 3,

        Psu = 4,

        Motherboard = 5,

        Case = 6,

        InternalDrives = 7,

        Cooler = 9,
    }
}
