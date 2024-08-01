using Medicines.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ImportDtos
{
    [XmlType("Medicine")]
    public class ImportMedicineDto
    {
        [XmlAttribute("category")]
        [Required]
        [Range(0,4)]
        public string Category { get; set; } = null!;

        [XmlElement(nameof(Name))]
        [Required]
        [MinLength(3)]
        [MaxLength(150)]
        public string Name { get; set; } = null!;

        [XmlElement(nameof(Price))]
        [Required]
        [Range(0.01,1000.00)]
        public double Price { get; set; }

        [XmlElement(nameof(ProductionDate))]
        [Required]
        public string ProductionDate { get; set; } = null!;

        [XmlElement(nameof(ExpiryDate))]
        [Required]
        public string ExpiryDate { get; set; } = null!;

        [XmlElement(nameof(Producer))]
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Producer { get; set; } = null!;
    }
}
