using TravelAgency.Data;
using TravelAgency.Data.Models.Enums;
using TravelAgency.DataProcessor.ExportDtos;

namespace TravelAgency.DataProcessor
{
    public class Serializer
    {
        public static string ExportGuidesWithSpanishLanguageWithAllTheirTourPackages(TravelAgencyContext context)
        {
            var allGuides = context.Guides
                .Where(g => g.Language == Language.Spanish)
                .OrderByDescending(g => g.TourPackagesGuides.Count())
                .ThenBy(g => g.FullName)
                .Select(g => new ExportGuideXmlDto
                {
                    FullName = g.FullName,
                    TourPackages = g.TourPackagesGuides
                    .Select(g => new ExportTourPackageXmlDto
                    { 
                        Name = g.TourPackage.PackageName,
                        Description = g.TourPackage.Description,    
                        Price = g.TourPackage.Price,
                    
                    })
                    .OrderByDescending(g => g.Price)
                    .ThenBy(g => g.Name)
                    .ToArray()
                })
                .ToArray();



            return XmlSerializationHelper.Serialize(allGuides, "Guides") ;
        }

        public static string ExportCustomersThatHaveBookedHorseRidingTourPackage(TravelAgencyContext context)
        {
            return "";
        }
    }
}
