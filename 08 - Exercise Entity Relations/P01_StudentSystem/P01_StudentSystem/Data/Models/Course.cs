
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace P01_StudentSystem.Data.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required]
        [MaxLength(80)]
        public string Name { get; set; } = null!;

        [MaxLength(128)]
        [Unicode]
        public string Description { get; set; } = null!;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]  
        public DateTime EndDate { get; set; }

        [Required]
        public decimal Price { get; set; }

        public virtual ICollection<Resource> Resources { get; set; } = null!;

        public virtual ICollection<Homework> Homeworks { get; set; } = null!;   

        public virtual ICollection<StudentCourse> StudentsCourses { get; set; } = null!;     

    }
}
