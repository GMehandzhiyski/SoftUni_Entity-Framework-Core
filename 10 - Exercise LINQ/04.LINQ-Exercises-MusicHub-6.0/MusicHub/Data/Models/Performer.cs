using MusicHub.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicHub.Data.Models
{
    public class Performer
    {
        public Performer()
        {
            PerformersSongs = new HashSet<SongPerformer>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.PerformerFirstNameMaxLenght)]
        public string? FirstName { get; set; }

        [Required]
        [MaxLength(ValidationConstants.PerformerLastNameMaxLenght)]
        public string? LastName { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public decimal NetWorth  { get; set; }

        public virtual ICollection<SongPerformer>? PerformersSongs { get; set; }



    }
}
