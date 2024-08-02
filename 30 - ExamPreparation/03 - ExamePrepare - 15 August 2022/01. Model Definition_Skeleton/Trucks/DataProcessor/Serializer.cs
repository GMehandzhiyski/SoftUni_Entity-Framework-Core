namespace Trucks.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using Trucks.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            var despatcherSerialixer = context.Despatchers
                .Where(d => d.Trucks.Any())
                .OrderByDescending(t => t.Trucks.Count())
                .ThenBy(d => d.Name)
                .Select(d => new ExportDespatcherXlmDto()
                {
                    TrucksCount = d.Trucks.Count(),
                    DespatcherName = d.Name,
                    Trucks = d.Trucks
                        .OrderBy(t => t.RegistrationNumber)
                        .Select(t => new ExportTruckXmlDto()
                        {
                            RegistrationNumber = t.RegistrationNumber,  
                            Make = t.MakeType.ToString(),
                        })
                        .ToArray()

                })
                .ToArray();

            return XmlSerializationHelper.Serialize(despatcherSerialixer, "Despatchers");
        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
            var clientsSerializer = context.Clients
                .Where(c => c.ClientsTrucks.Any(t => t.Truck.TankCapacity >= capacity))
                .ToArray()
                .Select(c => new ExportClientDto()
                { 
                    Name = c.Name,
                    Trucks = c.ClientsTrucks
                        .Where(ct => ct.Truck.TankCapacity >= capacity)
                        .OrderBy(c => c.Truck.MakeType)
                        .ThenByDescending(c => c.Truck.CargoCapacity)
                        .Select(ct => new ExportTruckDto()
                        { 
                            TruckRegistrationNumber = ct.Truck.RegistrationNumber,
                            VinNumber = ct.Truck.VinNumber,
                            TankCapacity = ct.Truck.TankCapacity,
                            CargoCapacity = ct.Truck.CargoCapacity,
                            CategoryType = ct.Truck.CategoryType.ToString(),
                            MakeType = ct.Truck.MakeType.ToString(),
                        
                        })
                        .ToArray()
                
                })
                .OrderByDescending(c => c.Trucks.Length)
                .ThenBy(c => c.Name)
                .Take(10)
                .ToArray();


            return JsonConvert.SerializeObject(clientsSerializer, Formatting.Indented);
        }
    }
}
