﻿
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SoftUni.Models
{
    public partial class Project
    {
        public Project()
        {
            EmployeesProjects = new HashSet<EmployeeProject>();
        }

        [Key]
        [Column("ProjectID")]
        public int ProjectId { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column(TypeName = "ntext")]
        public string? Description { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? EndDate { get; set; }

        //[ForeignKey("ProjectId")]
        //[InverseProperty(nameof(Employee.Projects))]
        public virtual ICollection<EmployeeProject> EmployeesProjects { get; set; }
    }
}
