using System.ComponentModel.DataAnnotations;

namespace TravelAgency.DataProcessor.ExportDtos
{
    public class ExportBookingDto
    {
        [Required]
        public string TourPackageName { get; set; } = null!;

        [Required]
        public string Date { get; set; } = null!;
    }
}
