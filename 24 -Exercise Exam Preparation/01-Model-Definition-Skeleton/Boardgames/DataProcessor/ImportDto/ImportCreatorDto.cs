using Boardgames.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType(nameof(Creator))]
    public class ImportCreatorDto
    {
        [XmlElement(nameof(FirstName))]
        [Required]
        [MinLength(2)]
        [MaxLength(7)]
        public string FirstName { get; set; } = null!;

        [XmlElement(nameof(LastName))]
        [Required]
        [MinLength(2)]
        [MaxLength(7)]
        public string LastName { get; set; } = null!;

        [XmlArray("Boardgames")]
        public ImportBoardGameDto[] Boardgames { get; set; }
    }
}
