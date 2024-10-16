﻿namespace HardwareStore.Core.ViewModels.Motherboard
{
    using HardwareStore.Core.Infrastructure.Attributes;
    using HardwareStore.Core.ViewModels.Product;

    [Category("Motherboard")]
    public class MotherboardViewModel : ProductViewModel
    {
        [Characteristic]
        public string Socket { get; set; } = null!;

        [Characteristic("Form Factor")]
        public string FormFactor { get; set; } = null!;

        [Characteristic]
        public string Chipset { get; set; } = null!;

        [Characteristic("Supported memory")]
        public string SupportedMemory { get; set; } = null!;

        [Characteristic("Built-in Wi-Fi")]
        public string BuiltInWIFI { get; set; } = null!;

        [Characteristic]
        public string Backlight { get; set; } = null!;
    }
}
