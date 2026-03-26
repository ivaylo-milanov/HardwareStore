namespace HardwareStore.Core.ViewModels.Admin
{
    using System.ComponentModel.DataAnnotations;
    using HardwareStore.Common;

    public class AssemblyComponentInputModel
    {
        public AssemblyRoleKind RoleKind { get; set; } = AssemblyRoleKind.None;

        [MaxLength(GlobalConstants.AssemblyComponentRoleMaxLength)]
        public string? CustomRole { get; set; }

        public int ComponentProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;
    }
}
