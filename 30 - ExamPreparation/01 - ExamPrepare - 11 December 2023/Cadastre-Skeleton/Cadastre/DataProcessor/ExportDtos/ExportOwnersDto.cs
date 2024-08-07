using Cadastre.Data.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace Cadastre.DataProcessor.ExportDtos
{
    public class ExportOwnersDto
    {
        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        public string LastName { get; set; } = null!;

        [Required]
        public string MaritalStatus { get; set; } = null!;
    }
}
