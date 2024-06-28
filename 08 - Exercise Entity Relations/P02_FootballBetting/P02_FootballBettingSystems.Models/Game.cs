using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBettingSystems.Models
{
    public class Game
    {

        public Game() 
        {
            Bets = new HashSet<Bet>();
        }
        [Key]
        public int GameId { get; set; }

        public int HomeTeamId { get; set; }
        [ForeignKey(nameof(HomeTeamId))]
        //public Team Team { get; set; } = null!;   

        public int AwayTeamId { get; set; }
        [ForeignKey(nameof(AwayTeamId))]
        public Team Team { get; set; } = null!;

        public int HomeTeamGoals { get; set; }

        public int AweyTeamGoals { get; set; }

        public DateTime DateTime { get; set; }

        public int HomeTeamBetRate { get; set; }

        public int AwayTeamBetRate { get; set; }

        public int DrawBetRate { get; set; }

        public int Result { get; set; }

        public virtual ICollection<Bet> Bets { get; set; }
        public virtual ICollection<PlayerStatistic> PlayersStatistics { get; set; } = null!;

    }
}
