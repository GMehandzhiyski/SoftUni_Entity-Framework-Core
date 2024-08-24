using System.ComponentModel.DataAnnotations;

namespace NetPay.DataProcessor.ExportDtos
{
    public class ExportServicedDto
    {
        [Required]
        //[MinLength(5)]
        //[MaxLength(30)]
        public string ServiceName { get; set; } = null!;

        [Required]
        public ExportSuppliersDto[] Suppliers { get; set; } = null!;    


    }
}
