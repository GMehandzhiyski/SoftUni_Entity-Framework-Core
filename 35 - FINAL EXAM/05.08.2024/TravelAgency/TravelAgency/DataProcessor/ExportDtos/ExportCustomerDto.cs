using System.ComponentModel.DataAnnotations;

namespace TravelAgency.DataProcessor.ExportDtos
{
    public class ExportCustomerDto
    {
        [Required]
      
        public string FullName { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;

        public ExportBookingDto[] Bookings { get; set; } = null!;

    }
}
