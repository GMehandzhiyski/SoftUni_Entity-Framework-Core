using Invoices.Data.Models;
using Microsoft.Extensions.Caching.Memory;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ImportDto
{
    [XmlType("Client")]
    public class ImportClientsDto
    {
    
        [XmlElement(nameof(Name))]
        [Required]
        [MinLength(10)]
        [MaxLength(25)]
        public string Name { get; set; } = null!;

        [XmlElement(nameof(NumberVat))]
        [Required]
        [MinLength(10)]
        [MaxLength(15)]
        public string NumberVat { get; set; } = null!;

        [XmlArray("Addresses")]
        public ImportAddressDto[] Address { get; set; } = null!;
    }
}
