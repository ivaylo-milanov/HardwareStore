namespace HardwareStore.Core.ViewModels.Admin
{
    /// <summary>
    /// Standard PC assembly slots. Use <see cref="Custom"/> for extra parts (e.g. cooler, Wi‑Fi).
    /// </summary>
    public enum AssemblyRoleKind
    {
        None = 0,

        Cpu = 1,

        Gpu = 2,

        Ram = 3,

        Psu = 4,

        Motherboard = 5,

        Case = 6,

        InternalDrives = 7,

        Custom = 8,

        Cooler = 9,
    }
}
