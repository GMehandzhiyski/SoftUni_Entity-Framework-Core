using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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




    }
}
