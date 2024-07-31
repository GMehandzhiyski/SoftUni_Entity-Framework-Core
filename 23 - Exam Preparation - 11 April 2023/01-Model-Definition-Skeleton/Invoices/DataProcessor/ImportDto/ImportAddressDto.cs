using Invoices.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ImportDto
{
    [XmlType(nameof(Address))] 
    public class ImportAddressDto
    {
        [XmlElement(nameof(StreetName))]
        [Required]
        [MinLength(10)]
        [MaxLength(20)]
        public string StreetName { get; set; } = null!;

        [XmlElement(nameof(StreetNumber))]
        [Required]
        public int StreetNumber { get; set; }

        [XmlElement(nameof(PostCode))]
        [Required]
        public string PostCode { get; set; } = null!;

        [XmlElement(nameof(City))]
        [Required]
        [MinLength(5)]
        [MaxLength(15)]
        public string City { get; set; } = null!;

        [XmlElement(nameof(Country))]
        [Required]
        [MinLength(5)]
        [MaxLength(15)]
        public string Country { get; set; } = null!;
    }
}
