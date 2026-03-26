namespace HardwareStore.Infrastructure.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using HardwareStore.Common;
    using Microsoft.EntityFrameworkCore;

    [Comment("product assembly bill of materials row")]
    public class ProductAssemblyComponent
    {
        [Comment("assembly component row id")]
        [Key]
        public int Id { get; set; }

        [Comment("assembled product id (parent)")]
        public int AssemblyProductId { get; set; }

        [Comment("assembled product")]
        [ForeignKey(nameof(AssemblyProductId))]
        public Product AssemblyProduct { get; set; } = null!;

        [Comment("component product id")]
        public int ComponentProductId { get; set; }

        [Comment("component product")]
        [ForeignKey(nameof(ComponentProductId))]
        public Product ComponentProduct { get; set; } = null!;

        [Comment("component slot role label e.g. CPU, GPU")]
        [Required]
        [MaxLength(GlobalConstants.AssemblyComponentRoleMaxLength)]
        public string Role { get; set; } = null!;

        [Comment("units of this component in the assembly")]
        public int Quantity { get; set; } = 1;

        [Comment("display order within the assembly")]
        public int SortOrder { get; set; }
    }
}
