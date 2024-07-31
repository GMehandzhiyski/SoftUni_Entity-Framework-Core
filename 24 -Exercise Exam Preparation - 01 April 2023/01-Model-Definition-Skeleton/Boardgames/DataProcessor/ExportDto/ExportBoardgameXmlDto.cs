using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto
{
    [XmlType("Boardgame")]
    public class ExportBoardgameXmlDto
    {
        [XmlElement(nameof(BoardgameName))]
        public string BoardgameName { get; set; } = null!;

        [XmlElement(nameof(BoardgameYearPublished))]
        public int BoardgameYearPublished { get; set; }

    }
}
