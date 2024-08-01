namespace Trucks.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.CompilerServices;
    using System.Text;
    using Castle.Core.Internal;
    using Data;
    using Newtonsoft.Json;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            var despatcherDeserialize = XmlSerializationHelper
                .Deserialize <ImportDespatcherXmlDto[]>(xmlString, "Despatchers");

            HashSet<Despatcher> despatchers = new HashSet<Despatcher>();

            foreach (var despatcherDto in despatcherDeserialize)
            {
                if (!IsValid(despatcherDto))
                { 
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                //if (string.IsNullOrEmpty(despatcherDto.Position))
                //{
                //    sb.AppendLine(ErrorMessage);
                //    continue;
                //}

                Despatcher newDespatcher = new Despatcher()
                {
                    Name = despatcherDto.Name,
                    Position = despatcherDto.Position,
                };


                foreach (var truckDto in despatcherDto.Trucks)
                {
                    if (!IsValid(truckDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (string.IsNullOrEmpty(truckDto.VinNumber)
                        || string.IsNullOrEmpty(truckDto.RegistrationNumber))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Truck newTruck = new Truck()
                    { 
                        RegistrationNumber = truckDto.RegistrationNumber,
                        VinNumber = truckDto.VinNumber,
                        TankCapacity = truckDto.TankCapacity,
                        CargoCapacity = truckDto.CargoCapacity, 
                        CategoryType = (CategoryType)truckDto.CategoryType,
                        MakeType = (MakeType)truckDto.MakeType
                    };


                    newDespatcher.Trucks.Add(newTruck);

                }

                despatchers.Add(newDespatcher);
                sb.AppendLine(string.Format(SuccessfullyImportedDespatcher, newDespatcher.Name, newDespatcher.Trucks.Count()));
            }

            context.Despatchers.AddRange(despatchers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
           StringBuilder sb = new StringBuilder();

            var clientDeserialize = JsonConvert
                .DeserializeObject<ImportClientDto[]>(jsonString);

            HashSet<Client> clients = new HashSet<Client>();

            foreach (var clientDto in clientDeserialize)
            {
                if (!IsValid(clientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (clientDto.Type == "usual")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Client newClient = new Client()
                { 
                    Name = clientDto.Name,
                    Nationality = clientDto.Nationality,
                    Type = clientDto.Type,
                };



                foreach (var truckDto in clientDto.Trucks.Distinct())
                {
                    if ((context.Trucks.Find(truckDto)) == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }


                    ClientTruck clientTruck = new ClientTruck()
                    {
                        TruckId = truckDto
                    };

                    newClient.ClientsTrucks.Add(clientTruck);
                }

                clients.Add(newClient);
                sb.AppendLine(string.Format(SuccessfullyImportedClient,newClient.Name, newClient.ClientsTrucks.Count()));
                
            }

            context.Clients.AddRange(clients);
            context.SaveChanges();

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