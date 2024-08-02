using System.Xml.Serialization;

namespace Trucks.DataProcessor.ExportDto
{
    [XmlType("Despatcher")]
    public class ExportDespatcherXlmDto
    {
        [XmlAttribute("TrucksCount")]
        public int TrucksCount { get; set; }

        [XmlElement("DespatcherName")]
        public string DespatcherName { get; set; } = null!;

        [XmlArray(nameof(Trucks))]
        public ExportTruckXmlDto[] Trucks { get; set; } = null!;
    }
}
