using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgency.Data.Models
{
    public class TourPackageGuide
    {
        [Required]
        public int TourPackageId { get; set; }
        [ForeignKey(nameof(TourPackageId))]

        [Required]
        public virtual TourPackage TourPackage { get; set; } = null!;

        public int GuideId { get; set; }
        [ForeignKey(nameof(GuideId))]

        [Required]
        public virtual Guide Guide { get; set; }
    }
}
