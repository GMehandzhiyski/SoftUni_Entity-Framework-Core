using NetPay.Data.Models.Enums;
using NetPay.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace NetPay.DataProcessor.ImportDtos
{
    public class ImportExpenseDto
    {
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        //[JsonProperty("BookingDate")]
        public string ExpenseName { get; set; } = null!;

        [Required]
        //[MaxLength(100_000)]
        [Range(0.01, 100000)]
        public decimal Amount { get; set; } 

        [Required]
        public string DueDate { get; set; } = null!;

        [Required]
        //[Range(1, 4)]
        public string PaymentStatus { get; set; } = null!;

        [Required]
        public int HouseholdId { get; set; }
     

        [Required]
        public int ServiceId { get; set; }
    }
}
