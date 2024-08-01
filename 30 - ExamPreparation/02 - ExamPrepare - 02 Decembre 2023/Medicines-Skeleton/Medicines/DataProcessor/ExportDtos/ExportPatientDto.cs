using Medicines.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ExportDtos
{
    [XmlType("Patient")]
    public class ExportPatientDto
    {
        [XmlAttribute(nameof(Gender))]
        [Required]
        public string Gender { get; set; } = null!;

        [XmlElement("Name")]
        [Required]
        public string FullName { get; set; } = null!;

        [XmlElement(nameof(AgeGroup))]
        [Required]
        public AgeGroup AgeGroup { get; set; }

        [XmlArray(nameof(Medicines))]
        public ExportMedicineDto[] Medicines { get; set; } = null!;
    }
}
