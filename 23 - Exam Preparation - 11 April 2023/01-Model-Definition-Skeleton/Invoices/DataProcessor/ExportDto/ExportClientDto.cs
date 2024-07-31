using Invoices.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ExportDto
{
    [XmlType(nameof(Client))]
    public class ExportClientDto
    {
        [XmlAttribute(nameof(InvoicesCount))]
        [Required]
        public int InvoicesCount { get; set; }

        [XmlElement(nameof(ClientName))]
        [Required]
        [MinLength(10)]
        [MaxLength(25)]
        public string ClientName { get; set; } = null!;

        [XmlElement(nameof(VatNumber))]
        [Required]
        [MinLength(10)]
        [MaxLength(15)]
        public string VatNumber { get; set; } = null!;

        [XmlArray(nameof(Invoices))]

        public ExportInvoiceDto[] Invoices { get; set; } = null!;   


    }
}
