using Cadastre.Data;
using Cadastre.DataProcessor.ExportDtos;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using System.Globalization;

namespace Cadastre.DataProcessor
{
    public class Serializer
    {
        public static string ExportPropertiesWithOwners(CadastreContext dbContext)
        {
            string constDate = "01/01/2000";
            DateTime constDateTime = DateTime
                .ParseExact(constDate,"dd/MM/yyyy",
                CultureInfo.InvariantCulture);

            var allProperties = dbContext.Properties
                .Where(p => p.DateOfAcquisition >= constDateTime)
                .OrderByDescending(p => p.DateOfAcquisition)
                .ThenBy(p => p.PropertyIdentifier)
                .Select(p => new ExportPropertyDto
                {
                    PropertyIdentifier = p.PropertyIdentifier,
                    Area = p.Area,
                    Address = p.Address,
                    DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy"),
                    Owners = p.PropertiesCitizens
                    .OrderBy(p => p.Citizen.LastName)
                    .Select(o => new ExportOwnersDto()
                    {
                        LastName = o.Citizen.LastName,
                        MaritalStatus = o.Citizen.MaritalStatus.ToString()
                    })
                    .ToArray()
                })
                .ToArray();

            return JsonConvert.SerializeObject(allProperties, Formatting.Indented);
        }

        public static string ExportFilteredPropertiesWithDistrict(CadastreContext dbContext)
        {
            var allProperties = dbContext.Properties
                .Where(p => p.Area >= 100)
                .OrderByDescending(p => p.Area)
                .ThenBy(p => p.DateOfAcquisition)
                .Select(p => new ExportPropertirsXmlDto 
                {
                    PostalCode = p.District.PostalCode,
                    PropertyIdentifier = p.PropertyIdentifier,
                    Area = p.Area,
                    DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy"),
                })
                .ToArray ();

            return XmlSerializationHelper.Serialize(allProperties, "Properties");
        }
    }
}
