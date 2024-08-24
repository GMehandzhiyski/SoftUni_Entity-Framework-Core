using NetPay.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace NetPay.DataProcessor.ExportDtos
{
    [XmlType("Household")]
    public class ExportHouseholdXmlDto
    {
        [XmlElement("ContactPerson")]
        [Required]
        // [MinLength(5)]
        //[MaxLength(50)]
        public string ContactPerson { get; set; } = null!;

        [XmlElement("Email")]
        //[MinLength(6)]
        //[MaxLength(80)]
        public string? Email { get; set; }

        [XmlElement("PhoneNumber")]
        [Required]
        [MaxLength(15)]
        //[RegularExpression()]
        public string PhoneNumber { get; set; } = null!;

        [XmlArray("Expenses")]
        public ExportExpenseXmlDto[] Expenses { get; set; } = null!;

    }
}
