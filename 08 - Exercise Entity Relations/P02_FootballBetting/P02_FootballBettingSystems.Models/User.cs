using P02_FootballBetting.Common;
using System.ComponentModel.DataAnnotations;


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
        [MaxLength(ValidationConstants.UserPasswordMaxLenght)]
        public string? Password { get; set; }

        public decimal Balance { get; set; }

        [MaxLength(ValidationConstants.NameMaxLenght)]
        public string? Name { get; set; }

        public virtual ICollection<Bet> Bets { get; set; } = null!;
    }
}
