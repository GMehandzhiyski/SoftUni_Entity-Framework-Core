using Medicines.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ExportDtos
{
    [XmlType("Medicine")]
    public class ExportMedicineXmlDto
    {
        [XmlAttribute("Category")]
        [Required]
        public string Category { get; set; } = null!;

        [XmlElement("Name")]
        [Required]
        //[MinLength(3)]
        //[MaxLength(150)]
        public string Name { get; set; } = null!;

        [XmlElement("Price")]
        [Required]
        //[MaxLength(1000)]
        //[Range(0,1000)]
        public string Price { get; set; }

        [XmlElement("Producer")]
        [Required]
        //[MinLength(3)]
        //[MaxLength(100)]
        public string Producer { get; set; } = null!;

        [XmlElement("BestBefore")]
        [Required]
        public string ExpiryDate { get; set; }= null!;


    }
}
