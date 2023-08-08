namespace HardwareStore.Infrastructure.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.EntityFrameworkCore;

    using Common;

    [Comment("characteristic table")]
    public class Characteristic
    {
        [Comment("characteristic product id")]
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        [Comment("characteristic product")]
        public virtual Product Product { get; set; } = null!;

        [Comment("characteristic name id")]
        [Required]
        public int CharacteristicNameId { get; set; }

        [Comment("characteristic name")]
        [ForeignKey(nameof(CharacteristicNameId))]
        public CharacteristicName CharacteristicName { get; set; } = null!;

        [Comment("characteristic value")]
        [Required]
        [MaxLength(GlobalConstants.CharacteristicValueMaxLength)]
        public string Value { get; set; } = null!;
    }
}
