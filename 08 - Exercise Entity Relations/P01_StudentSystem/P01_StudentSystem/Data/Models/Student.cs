using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace P01_StudentSystem.Data.Models
{
    public class Student
    {
        public Student()
        {
            this.StudentsCourses = new HashSet<StudentCourse>();
        }

        [Key]
        public int StudnetId { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MinLength(10)]
        [MaxLength(10)]
        public string? PhoneNumber { get; set; }

        [Required]
        public DateTime RegisteredOn { get; set; }
        public DateTime? Birthday { get; set; }

        public virtual ICollection<Homework> Homeworks { get; set; } = null!;
        public virtual ICollection<StudentCourse> StudentsCourses {  get; set; } = null!;   



    }
}
