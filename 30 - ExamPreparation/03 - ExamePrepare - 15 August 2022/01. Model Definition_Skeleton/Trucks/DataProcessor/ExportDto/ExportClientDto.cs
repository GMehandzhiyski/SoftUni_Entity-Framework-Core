using Castle.Components.DictionaryAdapter;
using System.ComponentModel.DataAnnotations;

namespace Trucks.DataProcessor.ExportDto
{
    public class ExportClientDto
    {
        [Required]
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        public ExportTruckDto[] Trucks { get; set; } = null!;
    }
}
