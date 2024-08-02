using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations;

namespace Artillery.Data.Models
{
    public class Country
    {
        public Country()
        {
            CountriesGuns = new List<CountryGun>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(60)]
        public string CountryName { get; set; } = null!;

        [Required]
        [MaxLength(10_000_000)]
        public int ArmySize  { get; set; }

        public virtual ICollection<CountryGun> CountriesGuns  { get; set;}

    }
}
