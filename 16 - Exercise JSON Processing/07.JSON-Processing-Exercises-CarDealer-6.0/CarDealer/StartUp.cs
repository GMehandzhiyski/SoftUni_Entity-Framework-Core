using CarDealer.Data;
using CarDealer.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();

            //01.
            string userText = File.ReadAllText("../../../Datasets/suppliers.json");
            Console.WriteLine(ImportSuppliers(context,userText));
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(inputJson);
            
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();


            return $"Successfully imported {suppliers.Count}.";
        }
    }
}