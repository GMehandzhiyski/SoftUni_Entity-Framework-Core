using P02_FootballBetting.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBettingSystems.Models
{
    public class Town
    {

        public Town() 
        {
            Players = new HashSet<Player>();
            Teams = new HashSet<Team>();    
        }

        [Key]
        public int TownId { get; set; }

        [Required]
        [MaxLength(ValidationConstants.TownNameMaxLenght)]
        public string Name { get; set; }

        public int CountryId { get; set; }
        [ForeignKey(nameof(CountryId))]
        public Country? Country { get; set; }

        public virtual ICollection<Team> Teams { get; set; } = null!;
        public virtual ICollection<Player> Players { get; set; } = null!;   

    }

  
}
