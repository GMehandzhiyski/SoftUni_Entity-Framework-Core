using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace P01_StudentSystem.Data.Models
{
    public class Student
    {
        [Key]
        public int StudnetId { get; set; }

        [Required]
        [MaxLength(100)]
        [Unicode]
        public string Name { get; set; } = null!;

        [MinLength(10)]
        [MaxLength(10)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public DateTime RegisteredOn { get; set; }

        public DateTime Birthday { get; set; }

        public virtual ICollection<Homework> Homeworks { get; set; } = null!;

        public virtual ICollection<StudentCourse> StudentsCourses {  get; set; } = null!;   



    }
}
