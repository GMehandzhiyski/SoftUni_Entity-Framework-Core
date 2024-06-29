using P02_FootballBetting.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBettingSystems.Models
{
    public class Team
    {
        public Team() 
        {
            this.HomeGames = new HashSet<Game>();
            this.AwayGames = new HashSet<Game>();
            this.Players = new HashSet<Player>();
        }

        [Key]
        public int TeamId { get; set; }

        [Required]
        [MaxLength(ValidationConstants.TeamNameLenght)]
        public string? Name { get; set; }

        [MaxLength(ValidationConstants.LogoUrlMaxLenght)]
        public string?  LogoUrl { get; set; }

        [Required]
        [MaxLength(ValidationConstants.InitialMaxLenght)]
        public string? Initials { get; set; }

        [Required]
        public decimal Budget { get; set; }

        public int PrimaryKitColorId { get; set; }
        [ForeignKey(nameof(PrimaryKitColorId))]
       public Color PrimaryKitColor { get; set; }    

        public int SecondaryKitColorId { get; set; }
        [ForeignKey(nameof(SecondaryKitColorId))]
        public Color SecondaryKitColor { get; set; }

        public int TownId { get; set; }
        [ForeignKey(nameof(TownId))]
        public Town Town { get; set; }

        
        public virtual ICollection<Game> HomeGames { get; set; } = null!;
        public virtual ICollection<Game> AwayGames { get; set; } = null!;
        public virtual ICollection<Player> Players { get; set; } = null!;




    }
}
