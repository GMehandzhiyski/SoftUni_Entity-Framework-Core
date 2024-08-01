using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Despatcher")]
    public class ImportDespatcherXmlDto
    {
        [XmlElement(nameof(Name))]
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        [XmlElement(nameof(Position))]
        [Required]
        public string Position { get; set; } = null!;  

        [XmlArray("Trucks")]
        public ImportTruckXmlDto[] Trucks { get; set; } = null!;
    }
}
