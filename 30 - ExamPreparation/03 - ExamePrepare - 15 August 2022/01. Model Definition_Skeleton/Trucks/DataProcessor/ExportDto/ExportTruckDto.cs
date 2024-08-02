using System.ComponentModel.DataAnnotations;
using Trucks.Data.Models.Enums;

namespace Trucks.DataProcessor.ExportDto
{
    public class ExportTruckDto
    {
        [MaxLength(8)]
        public string TruckRegistrationNumber { get; set; } = null!;

        [Required]
        [MaxLength(17)]
        public string VinNumber { get; set; } = null!;


        [MaxLength(1420)]
        public int TankCapacity { get; set; }

        [MaxLength(29000)]
        public int CargoCapacity { get; set; }

        [Required]
        public string CategoryType { get; set; } = null!;

        [Required]
        public string MakeType { get; set; } = null!;
    }
}
