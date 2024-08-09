using System.ComponentModel.DataAnnotations;

namespace Medicines.Data.Models
{
    public class Pharmacy
    {
        public Pharmacy()
        {
            Medicines = new HashSet<Medicine>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(14)]
        //[RegularExpression]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public bool IsNonStop  { get; set; }

        public virtual ICollection<Medicine> Medicines { get; set; }
    }
}
