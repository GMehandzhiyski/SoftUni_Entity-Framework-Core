using P02_FootballBetting.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBettingSystems.Models
{
    public class Player
    {
        public Player() 
        {
            PlayersStatistics = new HashSet<PlayerStatistic>();
        }

        [Key]
        public int PlayerId { get; set; }

        [Required]
        [MaxLength(ValidationConstants.PlayerNameMaxLenght)]
        public string? Name { get; set; }

        [Required]
        public int SquadNumber { get; set; }

        public int TownId { get; set; }
        [ForeignKey(nameof(TownId))]
        public Town Town { get; set; } = null!;

        public int PositionId { get; set; }
        [ForeignKey(nameof(PositionId))]
        public Position? Position { get; set; }

        public bool IsInjured { get; set; }

        public int TeamId { get; set; }

        public virtual ICollection<PlayerStatistic> PlayersStatistics { get; set; } = null!;
    }
}
