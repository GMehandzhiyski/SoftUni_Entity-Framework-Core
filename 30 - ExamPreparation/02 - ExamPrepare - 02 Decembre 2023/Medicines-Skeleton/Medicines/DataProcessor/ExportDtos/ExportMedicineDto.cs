using Medicines.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ExportDtos
{
    [XmlType("Medicine")]
    public class ExportMedicineDto
    {
        [XmlAttribute(nameof(Category))]
        [Required]
        public string Category { get; set; }

        [XmlElement(nameof(Name))]
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = null!;

        [XmlElement(nameof(Price))]
        [Required]
        [MaxLength(1000)]
        public string Price { get; set; } = null!;

        [XmlElement(nameof(Producer))]
        [Required]
        [MaxLength(100)]
        public string Producer { get; set; } = null!;

        [XmlElement("BestBefore")]
        [Required]
        public string BestBefore { get; set; } = null!;

    }
}
