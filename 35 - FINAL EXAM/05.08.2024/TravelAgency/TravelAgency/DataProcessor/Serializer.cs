using Newtonsoft.Json;
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
            var allcustomers = context.Customers
                .Where(c => c.Bookings.Any(b => b.TourPackage.PackageName == "Horse Riding Tour"))
                .Select(c => new ExportCustomerDto
                { 
                    FullName = c.FullName,
                    PhoneNumber = c.PhoneNumber,
                    Bookings = c.Bookings
                    .OrderBy(c => c.BookingDate)
                    .Where(c => c.TourPackage.PackageName == "Horse Riding Tour")
                    .Select(c => new ExportBookingDto
                    { 
                        TourPackageName = c.TourPackage.PackageName,    
                        Date = c.BookingDate.ToString("yyyy-MM-dd"),
                    
                    })
                    .ToArray()
                })
                .OrderByDescending(c => c.Bookings.Length)
                .ThenBy (c => c.FullName)
                .ToArray();

            return JsonConvert.SerializeObject(allcustomers, Formatting.Indented);
        }
    }
}
