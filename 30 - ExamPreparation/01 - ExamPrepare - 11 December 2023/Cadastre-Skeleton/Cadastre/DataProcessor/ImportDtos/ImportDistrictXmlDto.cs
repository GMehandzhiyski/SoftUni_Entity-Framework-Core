using Cadastre.Data.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ImportDtos
{
    [XmlType("District")]
    public class ImportDistrictXmlDto
    {
        [XmlAttribute("Region")]
        [Required]
        [MinLength(9)]
        public string Region { get; set; } = null!;

        [XmlElement("Name")]
        [Required]
        [MinLength(2)]
        [MaxLength(80)]
        public string Name { get; set; } = null!;

        [XmlElement("PostalCode")]
        [Required]
        [MaxLength(8)]
        [RegularExpression(@"^[A-Z]{2}-\d{5}$")]
        public string PostalCode { get; set; } = null!;

        [XmlArray("Properties")]
        
        public ImportPropartiesXmlDto[] Properties { get; set; } = null!;

    }
}
