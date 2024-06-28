﻿using Microsoft.EntityFrameworkCore;
using P02_FootballBettingSystems.Models;


namespace P02_FootballBetting.Data
{
    public class FootballBettingContext:DbContext
    {
            private string  ConnectionString = 
            "Server=.;Database=StudentSystem;User Id=sa;Password=Project123;TrustServerCertificate=true";

        public FootballBettingContext(DbContextOptions options)
            :base (options)
        {
                
        }
        
        public DbSet<Bet> Bets { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Country> Countries { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<PlayerStatistic> PlayersStatistics { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<User> Users { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
    }
    
}
