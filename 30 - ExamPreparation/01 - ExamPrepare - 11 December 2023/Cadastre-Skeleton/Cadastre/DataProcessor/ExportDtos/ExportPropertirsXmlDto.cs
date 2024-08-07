using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ExportDtos
{
    [XmlType("Property")]
    public class ExportPropertirsXmlDto
    {
        [XmlAttribute("postal-code")]
        [Required]
        public string PostalCode { get; set; } = null!;

        [XmlElement("PropertyIdentifier")]
        [Required]
        public string PropertyIdentifier { get; set; } = null!;

        [XmlElement("Area")]
        [Required]
        //not negative int
        public int Area { get; set; }

        [XmlElement("DateOfAcquisition")]
        [Required]
        public string DateOfAcquisition { get; set; } = null!;
    }
}
