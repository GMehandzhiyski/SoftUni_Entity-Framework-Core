using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using TravelAgency.Data.Models;

namespace TravelAgency.DataProcessor.ExportDtos
{
    [XmlType("Guide")]
    public class ExportGuideXmlDto
    {
        [XmlElement(nameof(FullName))]
        [Required]
        public string FullName { get; set; } = null!;

        [XmlArray(nameof(TourPackages))]
        public ExportTourPackageXmlDto[] TourPackages { get; set; } = null!;

    }
}
