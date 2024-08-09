using Medicines.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ExportDtos
{
    [XmlType("Patient")]
    public class ExportPatientXmlDto
    {
        [XmlAttribute("Gender")]
        [Required]
        public string Gender { get; set; } = null!;

        [XmlElement("Name")]
        [Required]
        //[MinLength(5)]
        //[MaxLength(100)]
        public string FullName { get; set; } = null!;

        [XmlElement("AgeGroup")]
        [Required]
        public string AgeGroup { get; set; } = null!;

        [XmlArray("Medicines")]
        public ExportMedicineXmlDto[] Medicines { get; set; } = null!;

    }
}
