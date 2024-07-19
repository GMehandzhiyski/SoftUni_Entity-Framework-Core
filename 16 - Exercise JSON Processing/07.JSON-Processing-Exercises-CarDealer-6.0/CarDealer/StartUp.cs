using CarDealer.Data;
using CarDealer.Models;
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
            string userText = File.ReadAllText("../../../Datasets/cars.json");
            Console.WriteLine(ImportCars(context, userText));

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
            var cars = JsonConvert.DeserializeObject<List<Car>>(inputJson);

            context.Cars.AddRange(cars);
            context.SaveChanges();


        return $"Successfully imported {cars.Count}.";
        }
    }
}