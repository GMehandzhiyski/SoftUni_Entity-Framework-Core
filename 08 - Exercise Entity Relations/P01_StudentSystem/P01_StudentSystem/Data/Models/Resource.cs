using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace P01_StudentSystem.Data.Models
{
    public class Resource
    {
        [Key]
        public int ResourceId { get; set; }

        [MaxLength(50)]
        public string Name { get; set; } = null!;
        public string? Url { get; set; }
        public ResourceType ResourceType { get; set; }

        public int CourseId { get; set; }

        [ForeignKey(nameof(CourseId))]
        public Course Course { get; set; } = null!;

    
    }

    public enum ResourceType
    {
        Video = 0,
        Presentation = 1,
        Document = 2,
        Other = 3,
    }

}
