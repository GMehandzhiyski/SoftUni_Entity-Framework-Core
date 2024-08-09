using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace Medicines.Data.Models
{
    public class Pharmacy
    {
        public Pharmacy()
        {
            Medicines = new List<Medicine>();    
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(14)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public  bool IsNonStop { get; set; }

        [Required]
        public virtual ICollection<Medicine> Medicines { get; set; } = null!;
    }
}
