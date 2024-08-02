using Footballers.Data.Models;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto
{
    [XmlType("Coach")]
    public class ExportCoachXmlDto
    {
        [XmlAttribute(nameof(FootballersCount))]
        public int FootballersCount { get; set; }

        [XmlElement(nameof(CoachName))]
        public string CoachName { get; set; } = null!;

        [XmlArray(nameof(Footballers))]
        public ExportFootballerXmlDto[] Footballers { get; set; } = null!;
    }
}
