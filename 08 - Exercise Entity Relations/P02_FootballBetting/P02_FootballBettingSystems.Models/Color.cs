using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBettingSystems.Models
{
    public class Color
    {
        public Color() 
        {
            Teams = new HashSet<Team>();
        } 
        public int ColorId { get; set; }

        public string? Name { get; set; }

        public virtual ICollection<Team> Teams { get; set; } = null!;   
    }
}
