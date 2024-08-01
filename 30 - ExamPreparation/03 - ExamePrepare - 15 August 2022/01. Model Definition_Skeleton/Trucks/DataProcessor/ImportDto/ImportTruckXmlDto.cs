using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Trucks.Data.Models.Enums;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Truck")]
    public class ImportTruckXmlDto
    {
        [XmlElement(nameof(RegistrationNumber))]
        [MaxLength(8)]
        [RegularExpression(@"^[A-Z]{2}\d{4}[A-Z]{2}$")]
        public string RegistrationNumber { get; set; } = null!;

        [XmlElement(nameof(VinNumber))]
        [Required]
        [MaxLength(17)]
        public string VinNumber { get; set; } = null!;

        [XmlElement(nameof(TankCapacity))]
        [Range(950, 1420)]
        public int TankCapacity { get; set; }

        [XmlElement(nameof(CargoCapacity))]
        [Range(5000, 29000)]
        public int CargoCapacity { get; set; }

        [XmlElement("CategoryType")]
        [Required]
        [Range(0, 3)]
        public int CategoryType { get; set; }

        [XmlElement("MakeType`")]
        [Required]
        [Range(0,4)]
        public int MakeType { get; set; }

    }
}
