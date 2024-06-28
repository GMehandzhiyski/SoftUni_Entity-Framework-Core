using P02_FootballBetting.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBettingSystems.Models
{
    public class User
    {
        public User() 
        {
            Bets = new HashSet<Bet>();
        }

        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(ValidationConstants.UserNameMaxLenght)]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }

        public decimal Balance { get; set; }

        public string? Name { get; set; }

        public virtual ICollection<Bet> Bets { get; set; } = null!;
    }
}
