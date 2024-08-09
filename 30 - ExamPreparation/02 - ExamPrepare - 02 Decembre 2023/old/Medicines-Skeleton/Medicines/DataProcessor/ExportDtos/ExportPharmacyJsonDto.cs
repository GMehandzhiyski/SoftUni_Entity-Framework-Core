using System.ComponentModel.DataAnnotations;

namespace Medicines.DataProcessor.ExportDtos
{
    public class ExportPharmacyJsonDto
    {
        public string Name { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;
    }
}
