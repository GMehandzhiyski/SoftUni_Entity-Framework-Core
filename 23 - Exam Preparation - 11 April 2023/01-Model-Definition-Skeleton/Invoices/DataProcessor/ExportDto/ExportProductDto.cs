using Invoices.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Invoices.DataProcessor.ExportDto
{
    public class ExportProductDto
    {
        //[Required]
        //[MaxLength(30)]
        public string Name { get; set; } = null!;

        //[Required]
        //[MaxLength(1000)]
        public decimal Price { get; set; }

        //[Required]
        public string Category { get; set; } = null!;

        public ExportJsonClientDto[] Clients { get; set; } = null!;
    }
}
