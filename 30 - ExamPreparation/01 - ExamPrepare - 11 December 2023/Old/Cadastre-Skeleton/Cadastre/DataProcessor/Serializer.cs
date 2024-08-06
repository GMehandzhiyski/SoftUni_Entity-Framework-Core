using Cadastre.Data;
using Cadastre.DataProcessor.ExportDtos;
using Newtonsoft.Json;
using System.Globalization;

namespace Cadastre.DataProcessor
{
    public class Serializer
    {
        public static string ExportPropertiesWithOwners(CadastreContext dbContext)
        {

            DateTime dateConst = DateTime.Parse("01/01/2000");

            var allProperties = dbContext.Properties
                .Where(p => p.DateOfAcquisition >= dateConst)
                .OrderByDescending(p => p.DateOfAcquisition)
                .ThenBy(p => p.PropertyIdentifier)
                .Select(p => new
                {
                    PropertyIdentifier = p.PropertyIdentifier,
                    Area = p.Area,
                    Address = p.Address,
                    DateOfAcquisition = p.DateOfAcquisition,
                    Owners = p.PropertiesCitizens
                        .Select(pc => new
                        {
                            LastName = pc.Citizen.LastName,
                            MaritalStatus = pc.Citizen.MaritalStatus.ToString(),
                        })
                         //.OrderBy(pc => pc.LastName)
                        .ToArray()
                })
                .AsEnumerable()
                .OrderByDescending(p => p.DateOfAcquisition)
                .ThenBy(p => p.PropertyIdentifier)
                .Select(p => new ExportPropertieDto
                {
                    PropertyIdentifier = p.PropertyIdentifier,
                    Area = p.Area,
                    Address = p.Address,
                    DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Owners = p.Owners
                        .Select(o => new ExportOwnerDto
                        {
                            LastName = o.LastName,
                            MaritalStatus = o.MaritalStatus,
                        })
                        .OrderBy(pc => pc.LastName)
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
                .Select(p => new ExportPropertyXmlDto()
                {
                    PostalCode = p.District.PostalCode,
                    PropertyIdentifier = p.PropertyIdentifier,
                    Area = p.Area,
                    DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy",CultureInfo.InvariantCulture)

                })
               
                .ToArray();
            
            return XmlSerializationHelper.Serialize(allProperties, "Properties");
        }
    }
}
