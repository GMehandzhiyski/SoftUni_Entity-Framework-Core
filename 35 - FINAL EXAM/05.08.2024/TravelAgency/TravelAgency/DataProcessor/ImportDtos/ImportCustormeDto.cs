using System.ComponentModel.DataAnnotations;
using System.Xml;
using System.Xml.Serialization;

namespace TravelAgency.DataProcessor.ImportDtos
{
    [XmlType("Customer")]
    public class ImportCustormeDto
    {
        [XmlAttribute("phoneNumber")]
        [Required]
        [MaxLength(13)]
        [RegularExpression(@"^\+\d{12}$")]
        public string phoneNumber { get; set; } = null!;

        [XmlElement(nameof(FullName))]
        [Required]
        [MinLength(4)]
        [MaxLength(60)]
        public string FullName { get; set; } = null!;

        [XmlElement(nameof(Email))]
        [Required]
        [MinLength(6)]
        [MaxLength(50)]
        public string Email { get; set; } = null!;

    }
}
