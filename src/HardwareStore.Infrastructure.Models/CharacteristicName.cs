namespace HardwareStore.Infrastructure.Models
{
    using HardwareStore.Common;
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;

    [Comment("characteristic name table")]
    public class CharacteristicName
    {
        public CharacteristicName()
        {
            this.Characteristics = new HashSet<Characteristic>();
        }

        [Comment("characteristic name id")]
        [Key]
        public int Id { get; set; }

        [Comment("characteristic name")]
        [Required]
        [MaxLength(GlobalConstants.CharacteristicNameMaxLength)]
        public string Name { get; set; } = null!;

        public ICollection<Characteristic> Characteristics { get; set; } = null!;
    }
}
