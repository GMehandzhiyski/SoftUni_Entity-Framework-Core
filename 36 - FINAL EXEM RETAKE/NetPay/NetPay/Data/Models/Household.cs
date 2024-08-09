using System.ComponentModel.DataAnnotations;

namespace NetPay.Data.Models
{
    public class Household
    {

        public Household()
        {
            Expenses = new HashSet<Expense>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
       // [MinLength(5)]
        [MaxLength(50)]
        public string ContactPerson { get; set; } = null!;

        //[MinLength(6)]
        [MaxLength(80)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(15)]
        //[RegularExpression()]
        public string PhoneNumber { get; set; } = null!;

        public virtual ICollection<Expense> Expenses  { get; set; }



    }
}
