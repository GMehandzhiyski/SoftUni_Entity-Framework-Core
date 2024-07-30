using Cadastre.Data.Enumerations;
using Cadastre.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ImportDtos
{
    [XmlType(nameof(District))]
    public class ImportDistrictsDto
    {
        [XmlAttribute(nameof(Region))]
        [Required]
        public Region Region { get; set; }

        [XmlElement(nameof(Name))]
        [Required]
        [MinLength(2)]
        [MaxLength(80)]
        public string Name { get; set; } = null!;

        [XmlElement(nameof(PostalCode))]
        [Required]
        [MaxLength(8)]
        [RegularExpression(@"^[A-Z]{2}-\d{5}$")]
        public string PostalCode { get; set; } = null!;

        public ImportPropertiesDto[] Properties { get; set; } = null!;
    }
}
