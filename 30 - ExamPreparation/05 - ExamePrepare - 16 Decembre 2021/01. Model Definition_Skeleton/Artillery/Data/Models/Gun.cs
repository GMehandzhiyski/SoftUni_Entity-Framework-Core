using Artillery.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artillery.Data.Models
{
    public class Gun
    {
        public Gun()
        {
            CountriesGuns = new List<CountryGun>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public int ManufacturerId { get; set; }
        [ForeignKey(nameof(ManufacturerId))]

        [Required]
        public virtual Manufacturer Manufacturer { get; set; } = null!;

        [Required]
        [MaxLength(1_350_000)]
        public int GunWeight { get; set; }

        [Required]
        [MaxLength(35)]
        public double BarrelLength  { get; set; }

        public int NumberBuild  { get; set; }

        [Required]
        [MaxLength(100_000)]
        public int Range  { get; set; }

        [Required]
        public GunType GunType { get; set; }

        [Required]
        public int ShellId  { get; set; }
        [ForeignKey(nameof(ShellId))]

        [Required]
        public virtual Shell Shell { get; set; } = null!;

        public virtual ICollection<CountryGun> CountriesGuns { get; set; }
    }
}
