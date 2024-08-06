using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cadastre.Data.Models
{
    public class PropertyCitizen
    {
        [Required]
        public int PropertyId  { get; set; }
        [ForeignKey(nameof(PropertyId))]

        [Required]
        public Property Property { get; set; } = null!;


        [Required]
        public int CitizenId  { get; set; }
        [ForeignKey(nameof(CitizenId))]

        [Required]
        public Citizen Citizen { get; set; } = null!;
    }
}
