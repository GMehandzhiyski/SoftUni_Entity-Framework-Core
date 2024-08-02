using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Coach")]
    public class ImportCoachXmlDto
    {
        [XmlElement(nameof(Name))]
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        [XmlElement(nameof(Nationality))]
        [Required]
        public string Nationality { get; set; } = null!;

        [XmlArray(nameof(Footballers))]

        public ImportFooltbollerXmlDto[] Footballers { get; set; } = null!;

    }
}
