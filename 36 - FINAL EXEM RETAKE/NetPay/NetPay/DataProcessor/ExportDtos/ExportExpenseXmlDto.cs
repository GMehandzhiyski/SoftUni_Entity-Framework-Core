using NetPay.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace NetPay.DataProcessor.ExportDtos
{
    [XmlType("Expense")]
    public class ExportExpenseXmlDto
    {
        [XmlElement("ExpenseName")]
        [Required]
        //[MinLength(5)]
        //MaxLength(50)]
        public string ExpenseName { get; set; } = null!;

        [XmlElement("Amount")]
        [Required]
        //[MaxLength(100_000)]
        // [Range(0.01,100_000)]
        public string Amount { get; set; } = null!;

        [XmlElement("PaymentDate")]
        [Required]
        public string DueDate { get; set; } = null!;

        [XmlElement("ServiceName")]
        [Required]
        public string ServiceName { get; set; }= null!;

    }
}
