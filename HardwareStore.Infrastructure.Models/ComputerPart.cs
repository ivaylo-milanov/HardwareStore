namespace HardwareStore.Infrastructure.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Comment("computer part table")]
    public class ComputerPart
    {
        [Comment("computer part id")]
        [Required]
        [ForeignKey(nameof(Part))]
        public int PartId { get; set; }

        [Comment("computer part")]
        public virtual Product Part { get; set; } = null!;

        [Comment("computer id")]
        [Required]
        [ForeignKey(nameof(Computer))]
        public int ComputerId { get; set; }

        [Comment("computer part")]
        public virtual Product Computer { get; set; } = null!;
    }
}
