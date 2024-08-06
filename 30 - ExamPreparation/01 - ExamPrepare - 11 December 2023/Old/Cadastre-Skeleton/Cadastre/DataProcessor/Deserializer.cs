namespace Cadastre.DataProcessor
{
    using Cadastre.Data;
    using Cadastre.Data.Enumerations;
    using Cadastre.Data.Models;
    using Cadastre.DataProcessor.ImportDtos;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid Data!";
        private const string SuccessfullyImportedDistrict =
            "Successfully imported district - {0} with {1} properties.";
        private const string SuccessfullyImportedCitizen =
            "Succefully imported citizen - {0} {1} with {2} properties.";

        public static string ImportDistricts(CadastreContext dbContext, string xmlDocument)
        {
            StringBuilder sb = new StringBuilder();

            var districtsDto = XmlSerializationHelper
                .Deserialize<ImportDistrictsDto[]>(xmlDocument, "Districts");

            HashSet<District> districts = new HashSet<District>();

            foreach (var districtDto in districtsDto)
            {
                if (!IsValid(districtDto))
                { 
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (dbContext.Districts.Any(d => d.Name == districtDto.Name))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }


                District newDistrict = new District()
                {
                    Region = (Region)Enum.Parse(typeof(Region),districtDto.Region),
                    Name= districtDto.Name,
                    PostalCode = districtDto.PostalCode,
                };

                HashSet<Property> properties = new HashSet<Property>();

                foreach (var propDto in districtDto.Properties)
                {
                    if (!IsValid(propDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;

                    }

                    var acquisitionDate = DateTime
                        .ParseExact(propDto.DateOfAcquisition, "dd/MM/yyyy", CultureInfo
                        .InvariantCulture, DateTimeStyles.None);
                    
                    if (dbContext.Properties.Any(p => p.PropertyIdentifier == propDto.PropertyIdentifier ) || newDistrict.Properties.Any(dp => dp.PropertyIdentifier == propDto.PropertyIdentifier))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (dbContext.Properties.Any(p => p.Address == propDto.Address) || newDistrict.Properties.Any(dp => dp.Address == propDto.Address))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Property property = new Property()
                    {
                        PropertyIdentifier = propDto.PropertyIdentifier,
                        Area = propDto.Area,
                        Details = propDto.Details,
                        Address = propDto.Address,
                        DateOfAcquisition = acquisitionDate

                    };

                    properties.Add(property);
                }
                
                newDistrict.Properties = properties;

                districts.Add(newDistrict);
                sb.AppendLine(string.Format(SuccessfullyImportedDistrict, districtDto.Name, newDistrict.Properties.Count));

            }

            dbContext.Districts.AddRange(districts);
            dbContext.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCitizens(CadastreContext dbContext, string jsonDocument)
        {
           StringBuilder sb = new StringBuilder();

            var citizenDeserialize = JsonConvert
                .DeserializeObject<ImportCitizenDto[]>(jsonDocument);

            HashSet<Citizen> citizens  = new HashSet<Citizen>();    

            foreach (var citizenDto in citizenDeserialize)
            {
                if (!IsValid(citizenDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var birthDateDateTime = DateTime
                    .ParseExact(citizenDto.BirthDate, "dd-MM-yyyy", CultureInfo
                    .InvariantCulture, DateTimeStyles.None);

              
                Citizen newCitizen = new Citizen()
                {
                    FirstName = citizenDto.FirstName,
                    LastName = citizenDto.LastName,
                    BirthDate  = birthDateDateTime,
                    MaritalStatus = (MaritalStatus)Enum.Parse(typeof(MaritalStatus), citizenDto.MaritalStatus),
                };


                HashSet<PropertyCitizen> propertysCitizens = new HashSet<PropertyCitizen>();
                foreach (var propDto in citizenDto.Properties)
                {

                    if (!IsValid(propDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    PropertyCitizen property = new PropertyCitizen()
                    {
                        PropertyId = propDto
                    };

                    propertysCitizens.Add(property);
                }

                newCitizen.PropertiesCitizens = propertysCitizens;
                citizens.Add(newCitizen);
                sb.AppendLine(string.Format(SuccessfullyImportedCitizen,
                                                newCitizen.FirstName,
                                                newCitizen.LastName,
                                                newCitizen.PropertiesCitizens.Count()));
            }
           
            dbContext.Citizens.AddRange(citizens);
            dbContext.SaveChanges();    

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
