namespace Cadastre.DataProcessor
{
    using Cadastre.Data;
    using Cadastre.Data.Enumerations;
    using Cadastre.Data.Models;
    using Cadastre.DataProcessor.ImportDtos;
    using Newtonsoft.Json;
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

            var districtDeserializer = XmlSerializationHelper
                .Deserialize<ImportDistrictXmlDto[]>(xmlDocument, "Districts");


            HashSet<District> districts = new HashSet<District>();

            foreach (var districtDto in districtDeserializer)
            {
                if (!IsValid(districtDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (districts.FirstOrDefault(n => n.Name == districtDto.Name) != null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!(districtDto.Region == Data.Enumerations.Region.SouthEast.ToString()
                    || districtDto.Region == Data.Enumerations.Region.NorthEast.ToString()
                    || districtDto.Region == Data.Enumerations.Region.NorthWest.ToString()
                    || districtDto.Region == Data.Enumerations.Region.SouthWest.ToString()))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Region regionEnum;
                bool isValidRegion = Enum.TryParse(districtDto.Region, true, out regionEnum);



                District newDistrict = new District()
                { 
                    Region = regionEnum,
                    Name = districtDto.Name,
                    PostalCode = districtDto.PostalCode,    
                };

                foreach (var propertirsDto in districtDto.Properties)
                {
                    if (!IsValid(propertirsDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime dateOfAcquisitionDateTime;
                    bool isDateOfAcquisitionValid = DateTime
                        .TryParseExact(propertirsDto.DateOfAcquisition, ("dd/MM/yyyy")
                        , CultureInfo.InvariantCulture
                        , DateTimeStyles.None
                        , out dateOfAcquisitionDateTime);

                    if (!isDateOfAcquisitionValid)
                    { 
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if ((dbContext.Properties.FirstOrDefault(p => p.PropertyIdentifier == propertirsDto.PropertyIdentifier) != null)
                        || districts.Any(p => p.Properties.FirstOrDefault(p => p.PropertyIdentifier == propertirsDto.PropertyIdentifier) != null))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }


                    if ((dbContext.Properties.FirstOrDefault(a => a.Address == propertirsDto.Address) != null)
                        || districts.Any(p => p.Properties.FirstOrDefault(a => a.Address == propertirsDto.Address) != null))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Property newProperty = new Property()
                    {
                        PropertyIdentifier = propertirsDto.PropertyIdentifier,
                        Area = propertirsDto.Area,
                        Details = propertirsDto.Details,
                        Address = propertirsDto.Address,
                        DateOfAcquisition = dateOfAcquisitionDateTime
                    };

                    newDistrict.Properties.Add(newProperty);
                }

                sb.AppendLine(string.Format(SuccessfullyImportedDistrict, newDistrict.Name, newDistrict.Properties.Count()));
                districts.Add(newDistrict);

            }
            dbContext.Districts.AddRange(districts);
            dbContext.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCitizens(CadastreContext dbContext, string jsonDocument)
        {
            StringBuilder sb = new StringBuilder();

            var citizenDeserializer = JsonConvert
                .DeserializeObject<ImportCitizenDto[]>(jsonDocument);

            HashSet<Citizen> citizens = new HashSet<Citizen>();

            foreach (var citizenDto in citizenDeserializer)
            {
                if (!IsValid(citizenDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime birthDateDateTime;
                bool isBirthDateValid = DateTime
                    .TryParseExact(citizenDto.BirthDate, "dd-MM-yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out birthDateDateTime);

                if (!isBirthDateValid) 
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!(citizenDto.MaritalStatus == MaritalStatus.Unmarried.ToString()
                    || citizenDto.MaritalStatus == MaritalStatus.Married.ToString()
                    || citizenDto.MaritalStatus == MaritalStatus.Divorced.ToString()
                    || citizenDto.MaritalStatus == MaritalStatus.Widowed.ToString()))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                MaritalStatus maritalStatus;
                bool isValid = Enum.TryParse(citizenDto.MaritalStatus,true, out maritalStatus); 

                Citizen newCitizen = new Citizen()
                {
                    FirstName = citizenDto.FirstName,
                    LastName = citizenDto.LastName,
                    BirthDate = birthDateDateTime,
                    MaritalStatus = maritalStatus
                };


                foreach (var propertyDto in citizenDto.Properties)
                {
                    PropertyCitizen newPropertyCitizen = new PropertyCitizen()
                    {
                        PropertyId = propertyDto,
                    };

                    newCitizen.PropertiesCitizens.Add(newPropertyCitizen);
                }

                citizens.Add(newCitizen);
                sb.AppendLine(String.Format(SuccessfullyImportedCitizen, newCitizen.FirstName, newCitizen.LastName, newCitizen.PropertiesCitizens.Count()));
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
