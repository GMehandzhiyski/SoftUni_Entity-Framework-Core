using System.ComponentModel.DataAnnotations;

namespace Cadastre.DataProcessor.ExportDtos
{
    public class ExportPropertyDto
    {
        [Required]
        [MinLength(16)]
        [MaxLength(20)]
        public string PropertyIdentifier { get; set; } = null!;

        [Required]
        //not negative int
        public int Area { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(200)]
        public string Address { get; set; } = null!;

        [Required]
        public string DateOfAcquisition { get; set; } = null!;

        public ExportOwnersDto[] Owners { get; set; } = null!;
    }
}
