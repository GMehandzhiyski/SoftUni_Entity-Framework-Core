using P02_FootballBetting.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBettingSystems.Models
{
    public class Country
    {
        public Country()
        {
            Towns =  new HashSet<Town>(); 
        }    

        [Key]
        public int CountryId { get; set; }

        [Required]
        [MaxLength(ValidationConstants.CountryNameMaxLenght)]
        public string? Name { get; set; }

        public virtual ICollection<Town> Towns { get; } = null!;
    }
}
