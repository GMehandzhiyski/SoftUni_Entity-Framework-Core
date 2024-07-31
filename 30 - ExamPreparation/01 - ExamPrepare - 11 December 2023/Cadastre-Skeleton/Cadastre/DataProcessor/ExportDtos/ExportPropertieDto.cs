using System.ComponentModel.DataAnnotations;

namespace Cadastre.DataProcessor.ExportDtos
{
    public class ExportPropertieDto
    {
        
        public string PropertyIdentifier { get; set; } = null!;

        public int Area { get; set; }

        public string Address { get; set; } = null!;

        public string DateOfAcquisition { get; set; } = null!;

        public ExportOwnerDto[] Owners { get; set; } = null!;
    }
}
