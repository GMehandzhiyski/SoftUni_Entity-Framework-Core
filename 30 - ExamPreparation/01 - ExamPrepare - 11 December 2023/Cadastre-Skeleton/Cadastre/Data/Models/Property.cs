using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace Cadastre.Data.Models
{
    public class Property
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(16)]
        [MaxLength(20)]
        public string PropertyIdentifier { get; set; } = null!;

        [Required]
        //not negative int
        public int Area { get; set; }

        [MinLength(5)]
        [MaxLength(500)]
        public string Details { get; set; } = null!;

        [Required]
        [MinLength(5)]
        [MaxLength(200)]
        public string Address { get; set; } = null!;

        [Required]
        public DateTime DateOfAcquisition  { get; set; }

        [Required]
        public int DistrictId  { get; set; }
        [ForeignKey(nameof(DistrictId))]

        [Required]
        public District District { get; set; } = null!;

        //colection
    }
}
