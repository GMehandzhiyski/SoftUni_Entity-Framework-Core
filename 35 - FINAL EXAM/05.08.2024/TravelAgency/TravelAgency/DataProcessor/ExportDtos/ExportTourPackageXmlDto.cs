using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace TravelAgency.DataProcessor.ExportDtos
{
    [XmlType("TourPackage")]
    public class ExportTourPackageXmlDto
    {
        [XmlElement("Name")]
        [Required]
        public string Name { get; set; } = null!;

        [XmlElement("Description")]
        [Required]
        public string Description { get; set; } = null!;

        [Required]
        [XmlElement("Price")]
        public decimal Price { get; set; }
    }
}
