using Medicines.Data.Models.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medicines.Data.Models
{
    public class Medicine
    {
        public Medicine()
        {
            PatientsMedicines = new List<PatientMedicine>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(1000)]
        public decimal Price { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        public DateTime ProductionDate  { get; set; }

        [Required]
        public DateTime ExpiryDate  { get; set; }

        [Required]
        [MaxLength(100)]
        public string Producer { get; set; } = null!;

        [Required]
        public int PharmacyId  { get; set; }
        [ForeignKey(nameof(PharmacyId))]

        [Required]
        public virtual Pharmacy Pharmacy { get; set; } = null!;

        [Required]
        public virtual ICollection<PatientMedicine> PatientsMedicines { get; set; } = null!;


    }
}
