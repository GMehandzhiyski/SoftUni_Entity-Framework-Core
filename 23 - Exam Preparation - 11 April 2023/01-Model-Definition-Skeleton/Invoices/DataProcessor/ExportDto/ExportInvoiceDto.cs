using Invoices.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ExportDto
{
    [XmlType("Invoice")]
    public class ExportInvoiceDto
    {

        [XmlElement(nameof(InvoiceNumber))]
        [Required]
        [Range(1_000_000_000, 1_500_000_000)]
        public int InvoiceNumber { get; set; }

        [XmlElement(nameof(InvoiceAmount))]
        [Required]
        public decimal InvoiceAmount { get; set; }

        [XmlElement(nameof(DueDate))]
        [Required]
        public string DueDate { get; set; } = null!;


        [XmlElement(nameof(Currency))]
        [Required]
        public string Currency { get; set; } = null!;
    }
}
