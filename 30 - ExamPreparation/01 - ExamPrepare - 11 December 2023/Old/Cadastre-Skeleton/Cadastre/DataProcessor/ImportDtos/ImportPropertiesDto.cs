using Cadastre.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ImportDtos
{
    [XmlType(nameof(Property))]
    public class ImportPropertiesDto
    {
        [XmlElement(nameof(PropertyIdentifier))]
        [Required]
        [MinLength(16)]
        [MaxLength(20)]
        public string PropertyIdentifier { get; set; } = null!;

        [XmlElement(nameof(Area))]
        [Required]
        [Range(0, int.MaxValue)]
        public int Area { get; set; }

        [XmlElement(nameof(Details))]
        [MinLength(5)]
        [MaxLength(500)]
        public string Details { get; set; } = null!;

        [XmlElement(nameof(Address))]
        [Required]
        [MinLength(5)]
        [MaxLength(200)]
        public string Address { get; set; } = null!;

        [XmlElement(nameof(DateOfAcquisition))]
        [Required]
        public string DateOfAcquisition { get; set; } = null!;
    }
}
