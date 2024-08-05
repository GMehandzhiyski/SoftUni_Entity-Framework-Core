using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using TravelAgency.Data.Models;

namespace TravelAgency.DataProcessor.ImportDtos
{
    [XmlType("Customer")]
    public class ImportCustomerXmlDto
    {
        [Required]
        [MaxLength(13)]
        [RegularExpression(@"^\+\d{12}$")]
        [XmlAttribute(nameof(phoneNumber))]
        public string phoneNumber { get; set; } = null!;

        [Required]
        [MinLength(4)]
        [MaxLength(60)]
        [XmlElement(nameof(FullName))]
        public string FullName { get; set; } = null!;

        [Required]
        [MinLength(6)]
        [MaxLength(50)]
        [XmlElement(nameof(Email))]
        public string Email { get; set; } = null!;

    }
}
