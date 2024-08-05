using Microsoft.EntityFrameworkCore.Query.Internal;
using System.ComponentModel.DataAnnotations;

namespace TravelAgency.Data.Models
{
    public class Customer
    {
        public Customer()
        {
            Bookings = new HashSet<Booking>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        //[MinLength(4)]
        [MaxLength(60)]
        public string FullName { get; set; } = null!;

        [Required]
        //[MinLength(6)]
        [MaxLength(50)]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(13)]
        public  string PhoneNumber { get; set; } = null!;

        public virtual ICollection<Booking> Bookings { get; set; } = null!;

    }
}
