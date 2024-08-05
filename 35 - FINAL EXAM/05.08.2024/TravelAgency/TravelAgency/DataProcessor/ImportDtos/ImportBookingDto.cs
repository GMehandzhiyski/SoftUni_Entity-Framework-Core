using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TravelAgency.DataProcessor.ImportDtos
{
    public class ImportBookingDto
    {
        [Required]
        [JsonProperty("BookingDate")]
        public string BookingDate { get; set; } = null!;

        [Required]
        [MinLength(4)]
        [MaxLength(60)]
        [JsonProperty("CustomerName")]
        public string CustomerName { get; set; } = null!;


        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        [JsonProperty("TourPackageName")]
        public string TourPackageName { get; set; } = null!;
    }
}
