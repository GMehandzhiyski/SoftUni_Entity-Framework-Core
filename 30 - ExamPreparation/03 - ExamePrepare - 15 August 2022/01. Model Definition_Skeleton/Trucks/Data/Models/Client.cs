using System.ComponentModel.DataAnnotations;

namespace Trucks.Data.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(40)]
        public string Nationality  { get; set; }

        [Required]
        public string Type  { get; set; }

        //Collection

    }
}
