using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Update;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();

            //09.
            //string userText = File.ReadAllText("../../../Datasets/suppliers.json");
            //Console.WriteLine(ImportSuppliers(context,userText));

            //10.
            //string userText = File.ReadAllText("../../../Datasets/parts.json");
            //Console.WriteLine(ImportParts(context, userText));

            //11.
            //string userText = File.ReadAllText("../../../Datasets/cars.json");
            //Console.WriteLine(ImportCars(context, userText));

            //12.
            //string userText = File.ReadAllText("../../../Datasets/customers.json");
            //Console.WriteLine(ImportCustomers(context, userText));

            //13.
            //string userText = File.ReadAllText("../../../Datasets/sales.json");
            //Console.WriteLine(ImportSales(context, userText));

            //14.
            Console.WriteLine(GetOrderedCustomers(context));
        }
        //09.
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(inputJson);
            
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}.";
        }

        //10.
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var parts = JsonConvert.DeserializeObject<List<Part>>(inputJson);

            var validSupplierId = context.Suppliers
                .Select(x => x.Id)
                .ToList();

            var partsImport = parts
                .Where(p => validSupplierId.Contains(p.SupplierId))
                .ToList();

            context.Parts.AddRange(partsImport);
            context.SaveChanges();

            return $"Successfully imported {partsImport.Count}.";
        }

        //11.
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var carsDtos = JsonConvert.DeserializeObject<List<ImportCarDto>>(inputJson);

            var cars = new HashSet<Car>();
            var partsCars = new HashSet<PartCar>();

            foreach (var carDto in carsDtos)
            {
                var newCar = new Car
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TraveledDistance = carDto.TraveledDistance
                };

                cars.Add(newCar);

                foreach (var partId in carDto.PartsId.Distinct())
                {
                    partsCars.Add(new PartCar()
                    {
                        Car = newCar,
                        PartId = partId

                    });
                   
                }
            }

            context.Cars.AddRange(cars);
            context.PartsCars.AddRange(partsCars);

            context.SaveChanges();



            return $"Successfully imported {cars.Count}.";
        }

        //12.
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customersDto = JsonConvert.DeserializeObject<List<ImportCustormDto>>(inputJson);

            var customers = new HashSet<Customer>();

            foreach (var customerDto in customersDto)
            {
                var customer = new Customer
                {
                    Name = customerDto.Name,
                    BirthDate = customerDto.BirthDate,
                    IsYoungDriver = customerDto.IsYoungDriver,
                };
                customers.Add(customer);
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}.";
        }

        //13.
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var salesDto = JsonConvert.DeserializeObject<List<ImportSaleDto>>(inputJson);

            var sales = new HashSet<Sale>();

            foreach (var curSalesDto in salesDto)
            {
                var sale = new Sale
                { 
                    Discount = curSalesDto.Discount,
                    CarId = curSalesDto.CarId,
                    CustomerId = curSalesDto.CustomerId,
                
                };

                sales.Add(sale);
            }

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}.";
        }

        //14.
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var orderedCustomers = context.Customers
             .OrderBy(c => c.BirthDate)
             .ThenBy(c => c.IsYoungDriver)
             .Select(c => new
             {
                 c.Name,
                 BirthDate = c.BirthDate.ToString("dd/MM/yyyy"),
                 c.IsYoungDriver
             })
             .ToList();

            var json = JsonConvert.SerializeObject(orderedCustomers, Formatting.Indented);
            return json;
        }
    }

}