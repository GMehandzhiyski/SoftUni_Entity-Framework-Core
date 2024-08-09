using Medicines.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Medicines.DataProcessor.ImportDtos
{
    public class ImportPatientsDto
    {
        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        [Range(0,2)]
        public string AgeGroup { get; set; } = null!;

        [Required]
        [Range(0,1)]
        public string Gender { get; set; } = null!;

        public int[] Medicines { get; set; }

    }
}
