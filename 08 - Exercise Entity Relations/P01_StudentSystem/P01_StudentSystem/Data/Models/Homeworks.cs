
using System.ComponentModel.DataAnnotations;

namespace P01_StudentSystem.Data.Models
{
    public class Homeworks
    {
        [Key]
        public int HomeworkId { get; set; }

        [MaxLength(280)]ninka e super
        public string Content  { get; set; }
    }
}
