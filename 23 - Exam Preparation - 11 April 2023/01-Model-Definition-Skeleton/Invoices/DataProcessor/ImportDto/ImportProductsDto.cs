using Invoices.Data.Models.Enums;
using Invoices.Data.Models;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Invoices.DataProcessor.ImportDto
{
    public class ImportProductsDto
    {


        [Required]
        [MinLength(9)]
        [MaxLength(30)]
        public string Name { get; set; } = null!;

        [Required]
        [Range(5,1000)]
        public decimal Price { get; set; }

        [Required]
        [Range(0,4)]
        public int CategoryType { get; set; }

        [Required]
        public int[] Clients { get; set; } = null!;
       
    }
}
