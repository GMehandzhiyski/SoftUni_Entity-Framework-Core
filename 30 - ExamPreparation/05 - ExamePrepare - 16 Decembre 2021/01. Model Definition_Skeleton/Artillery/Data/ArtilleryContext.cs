﻿namespace Artillery.Data
{
    using Artillery.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Reflection.Emit;

    public class ArtilleryContext : DbContext
    {
        public ArtilleryContext() 
        { 
        }

        public DbSet<Country> Countries { get; set; } = null!;

        public DbSet<CountryGun> CountriesGuns { get; set; } = null!; 

        public DbSet<Gun> Guns { get; set; } = null!;

        public DbSet<Manufacturer> Manufacturers { get; set; } = null!; 

        public DbSet<Shell> Shells { get; set; } = null!;

        public ArtilleryContext(DbContextOptions options)
            : base(options) 
        { 
        }

         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CountryGun>()
                .HasKey(cg => new { cg.CountryId, cg.GunId });

        }
    }
}
