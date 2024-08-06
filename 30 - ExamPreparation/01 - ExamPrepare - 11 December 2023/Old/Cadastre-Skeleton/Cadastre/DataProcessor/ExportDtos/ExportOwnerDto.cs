using Cadastre.Data.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace Cadastre.DataProcessor.ExportDtos
{
    public class ExportOwnerDto
    {
        public string LastName { get; set; } = null!;

        public string MaritalStatus { get; set; } = null!;
    }
}
