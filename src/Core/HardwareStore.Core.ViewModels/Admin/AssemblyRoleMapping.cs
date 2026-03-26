namespace HardwareStore.Core.ViewModels.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class AssemblyRoleMapping
    {
        public static AssemblyRoleKind FromStoredRole(string? role)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                return AssemblyRoleKind.None;
            }

            var s = role.Trim();
            if (string.Equals(s, "CPU", StringComparison.OrdinalIgnoreCase))
            {
                return AssemblyRoleKind.Cpu;
            }

            if (string.Equals(s, "GPU", StringComparison.OrdinalIgnoreCase))
            {
                return AssemblyRoleKind.Gpu;
            }

            if (string.Equals(s, "RAM", StringComparison.OrdinalIgnoreCase))
            {
                return AssemblyRoleKind.Ram;
            }

            if (string.Equals(s, "PSU", StringComparison.OrdinalIgnoreCase))
            {
                return AssemblyRoleKind.Psu;
            }

            if (string.Equals(s, "Motherboard", StringComparison.OrdinalIgnoreCase))
            {
                return AssemblyRoleKind.Motherboard;
            }

            if (string.Equals(s, "Case", StringComparison.OrdinalIgnoreCase))
            {
                return AssemblyRoleKind.Case;
            }

            if (string.Equals(s, "InternalDrives", StringComparison.OrdinalIgnoreCase))
            {
                return AssemblyRoleKind.InternalDrives;
            }

            if (string.Equals(s, "Cooler", StringComparison.OrdinalIgnoreCase))
            {
                return AssemblyRoleKind.Cooler;
            }

            return AssemblyRoleKind.Custom;
        }

        /// <summary>
        /// Category names that qualify a product for this assembly slot (case-insensitive match on <see cref="HardwareStore.Infrastructure.Models.Category.Name"/>).
        /// </summary>
        public static IReadOnlyList<string> CategoryNamesForFilter(AssemblyRoleKind kind) =>
            kind switch
            {
                AssemblyRoleKind.Cpu => new[] { "CPU", "Processor", "Processors" },
                AssemblyRoleKind.Gpu => new[] { "GPU", "Graphics", "Graphics card", "Graphics cards" },
                AssemblyRoleKind.Ram => new[] { "RAM", "Memory" },
                AssemblyRoleKind.Psu => new[] { "PSU", "Power supply", "Power supplies" },
                AssemblyRoleKind.Motherboard => new[] { "Motherboard", "Motherboards", "Mainboard", "Mainboards" },
                AssemblyRoleKind.Case => new[] { "Case", "Cases", "Chassis" },
                AssemblyRoleKind.InternalDrives => new[] { "Internal drives", "InternalDrives", "SSD", "HDD", "Storage", "NVMe" },
                AssemblyRoleKind.Cooler => new[] { "Cooler", "Coolers", "CPU Cooler", "CPU Coolers", "AIO" },
                _ => Array.Empty<string>(),
            };

        public static bool ProductCategoryMatchesRole(string? categoryName, AssemblyRoleKind roleKind)
        {
            if (roleKind == AssemblyRoleKind.None || roleKind == AssemblyRoleKind.Custom)
            {
                return true;
            }

            var names = CategoryNamesForFilter(roleKind);
            if (names.Count == 0)
            {
                return true;
            }

            var c = categoryName?.Trim() ?? string.Empty;
            return names.Any(n => c.Equals(n, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Returns the string persisted on <see cref="HardwareStore.Infrastructure.Models.ProductAssemblyComponent.Role"/>.
        /// </summary>
        public static string? ToPersistedRole(AssemblyRoleKind kind, string? customRole)
        {
            if (kind == AssemblyRoleKind.None)
            {
                return null;
            }

            if (kind == AssemblyRoleKind.Custom)
            {
                return string.IsNullOrWhiteSpace(customRole) ? null : customRole.Trim();
            }

            return kind switch
            {
                AssemblyRoleKind.Cpu => "CPU",
                AssemblyRoleKind.Gpu => "GPU",
                AssemblyRoleKind.Ram => "RAM",
                AssemblyRoleKind.Psu => "PSU",
                AssemblyRoleKind.Motherboard => "Motherboard",
                AssemblyRoleKind.Case => "Case",
                AssemblyRoleKind.InternalDrives => "InternalDrives",
                AssemblyRoleKind.Cooler => "Cooler",
                _ => null,
            };
        }
    }
}
