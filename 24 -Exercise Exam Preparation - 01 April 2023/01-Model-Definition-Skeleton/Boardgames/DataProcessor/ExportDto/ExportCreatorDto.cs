using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto
{
    [XmlType("Creator")]
    public class ExportCreatorDto
    {
        [XmlAttribute(nameof(BoardgamesCount))]
        [Required]
        public int BoardgamesCount { get; set; }

        [XmlElement(nameof(CreatorName))]
        [Required]
        [MaxLength(7)]
        public string CreatorName { get; set; } = null!;

        [XmlArray(nameof(Boardgames))]
        public ExportBoardgameXmlDto[] Boardgames { get; set; } = null!;
    }
}
