using Boardgames.Data.Models.Enums;
using Boardgames.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType(nameof(Boardgame))]
    public class ImportBoardGameDto
    {
        [XmlElement(nameof(Name))]
        [Required]
        [MinLength(10)]
        [MaxLength(20)]
        public string Name { get; set; } = null!;

        [XmlElement(nameof(Rating))]
        [Required]
        [Range(1, 10)]
        public double Rating { get; set; }

        [XmlElement(nameof(YearPublished))]
        [Required]
        [Range(2018,2023)]
        public int YearPublished { get; set; }

        [XmlElement(nameof(CategoryType))]
        [Required]
        public int CategoryType { get; set; }

        [XmlElement(nameof(Mechanics))]
        [Required]
        public string Mechanics { get; set; } = null!;

    }
}