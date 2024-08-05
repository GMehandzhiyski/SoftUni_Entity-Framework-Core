using System.ComponentModel.DataAnnotations;

namespace TravelAgency.DataProcessor.ImportDtos
{
    public class ImportBookingDto
    {
        public string BookingDate { get; set; } = null!;

        [Required]
        [MinLength(4)]
        [MaxLength(60)]
        public string CustomerName { get; set; } = null!;

        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string TourPackageName { get; set; } = null!;
    }
}
