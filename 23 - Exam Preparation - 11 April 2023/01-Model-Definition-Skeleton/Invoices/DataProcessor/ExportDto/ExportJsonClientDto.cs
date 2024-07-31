using System.ComponentModel.DataAnnotations;

namespace Invoices.DataProcessor.ExportDto
{
    public class ExportJsonClientDto
    {
        //[Required]
        //[MaxLength(25)]
        public string Name { get; set; } = null!;

        //[Required]
        //[MaxLength(15)]
        public string NumberVat { get; set; } = null!;
    }
}
