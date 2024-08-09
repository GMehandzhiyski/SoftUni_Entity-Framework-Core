using System.ComponentModel.DataAnnotations;

namespace Medicines.DataProcessor.ExportDtos
{
    public class ExportMedicineJsonDto
    {

        public string Name { get; set; } = null!;


        public string Price { get; set; }

        public ExportPharmacyJsonDto Pharmacy { get; set; }

    }
}
