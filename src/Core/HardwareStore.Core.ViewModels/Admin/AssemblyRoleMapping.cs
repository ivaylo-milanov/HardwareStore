namespace HardwareStore.Core.ViewModels.Admin
{
    using System;
    using HardwareStore.Infrastructure.Models.Enums;

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
        /// Maps a standard assembly role to the category assembly slot products must use (None if not a constrained role).
        /// </summary>
        public static CategoryAssemblySlot AssemblySlotForRole(AssemblyRoleKind kind) =>
            kind switch
            {
                AssemblyRoleKind.Cpu => CategoryAssemblySlot.Cpu,
                AssemblyRoleKind.Gpu => CategoryAssemblySlot.Gpu,
                AssemblyRoleKind.Ram => CategoryAssemblySlot.Ram,
                AssemblyRoleKind.Psu => CategoryAssemblySlot.Psu,
                AssemblyRoleKind.Motherboard => CategoryAssemblySlot.Motherboard,
                AssemblyRoleKind.Case => CategoryAssemblySlot.Case,
                AssemblyRoleKind.InternalDrives => CategoryAssemblySlot.InternalDrives,
                AssemblyRoleKind.Cooler => CategoryAssemblySlot.Cooler,
                _ => CategoryAssemblySlot.None,
            };

        public static bool ProductCategoryMatchesRole(CategoryAssemblySlot categorySlot, AssemblyRoleKind roleKind)
        {
            if (roleKind is AssemblyRoleKind.None or AssemblyRoleKind.Custom)
            {
                return true;
            }

            var expected = AssemblySlotForRole(roleKind);
            if (expected == CategoryAssemblySlot.None)
            {
                return true;
            }

            return categorySlot == expected;
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
