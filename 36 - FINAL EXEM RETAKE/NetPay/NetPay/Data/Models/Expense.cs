using NetPay.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetPay.Data.Models
{
    public class Expense
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string ExpenseName { get; set; } = null!;

        [Required]
        [MaxLength(100_000)]
       // [Range(0.01,100_000)]
        public decimal Amount  { get; set; }

        [Required]
        public DateTime DueDate  { get; set; }

        [Required]
        public PaymentStatus PaymentStatus  { get; set; }

        [Required]
        public int HouseholdId  { get; set; }
        [ForeignKey(nameof(HouseholdId))]

        [Required]
        public virtual Household Household { get; set; } = null!;

        [Required]
        public int ServiceId  { get; set; }
        [ForeignKey(nameof(ServiceId))]
        [Required]
        public virtual Service Service { get; set; } = null!;











    }
}
