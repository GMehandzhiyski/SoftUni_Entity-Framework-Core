using Microsoft.EntityFrameworkCore;
using TravelAgency.Data;
using TravelAgency.Data.Models.Enums;
using TravelAgency.DataProcessor.ExportDtos;

namespace TravelAgency.DataProcessor
{
    public class Serializer
    {
        public static string ExportGuidesWithSpanishLanguageWithAllTheirTourPackages(TravelAgencyContext context)
        {
            var allguidens = context.Guides
                .Where(g => g.Language == Language.Spanish)
               .OrderByDescending(c => c.TourPackagesGuides.Count())
               .ThenBy(c => c.FullName)
               .Select(g => new ExportGuideXmlDto
               {
                   FullName = g.FullName,
                   TourPackages = g.TourPackagesGuides
                    .Select(p => new ExportTourPackagesXmlDto()
                    {
                        Name = p.TourPackage.PackageName,
                        Description = p.TourPackage.Description,
                        Price = p.TourPackage.Price,
                    })
                    .ToArray()

               })
               .ToArray();
 

            return XmlSerializationHelper.Serialize(allguidens, "Guides");
        }

        public static string ExportCustomersThatHaveBookedHorseRidingTourPackage(TravelAgencyContext context)
        {
            return "";
        }
    }
}
