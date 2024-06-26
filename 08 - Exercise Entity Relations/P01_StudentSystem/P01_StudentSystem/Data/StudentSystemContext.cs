﻿using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        private const string ConnectionString =
           "Server=.;Database=StudentSystem;User Id=sa;Password=Project123;TrustServerCertificate=true";

        public StudentSystemContext(DbContextOptions dbContextOptions)
            :base(dbContextOptions) 
        {
            
        }



        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Homework> Homeworks { get; set; } = null!; 
        public DbSet<Resource> Resources { get; set; } = null!;
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<StudentCourse> StudentsCourses { get; set; } = null!;  

        ///<summary>
       /// </summary>>
       /// <param name = "optionsBuilder"></param>

        // SQL provider
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(ConnectionString);
           
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Primary key
            modelBuilder.Entity<StudentCourse>()
            .HasKey(sc => new
            {
                sc.StudentId,
                sc.CourseId
            });

            // not Unicode
            modelBuilder.Entity<Resource>()
                .Property(p => p.ResourceId)
                .IsUnicode(false);

            // not Unicode
            modelBuilder.Entity<Student>()
                .Property(s => s.PhoneNumber)
                .IsConcurrencyToken(false);
        }




    }
}
