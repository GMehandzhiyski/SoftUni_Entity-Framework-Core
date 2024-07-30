using Invoices.Data.Models.Enums;
using Invoices.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Invoices.DataProcessor.ImportDto
{

    public class ImportInvoiceDto
    {
        [Required]
        [MinLength(1_000_000_000)]
        [MaxLength(1_500_000_000)]
        public int Number { get; set; }

        [Required]
        public string IssueDate { get; set; } = null!;

        [Required]
        public string DueDate { get; set; } = null!;

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public CurrencyType CurrencyType { get; set; }

        [Required]
        public int ClientId { get; set; }

    }
}
