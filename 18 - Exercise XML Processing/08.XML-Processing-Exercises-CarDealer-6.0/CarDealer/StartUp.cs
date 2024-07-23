using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();

            //09.
            string userXml = File.ReadAllText("../../../Datasets/suppliers.xml");
            Console.WriteLine(ImportSuppliers(context, userXml));

            //10.
            //string userXml = File.ReadAllText("../../../Datasets/parts.xml");
            //Console.WriteLine(ImportParts(context, userXml));
        }

        //09.
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SupplierImportDto[]),
              new XmlRootAttribute("Suppliers"));
            SupplierImportDto[] importDtos;
            using (var reader = new StringReader(inputXml))
            {
                importDtos = (SupplierImportDto[])xmlSerializer.Deserialize(reader);
            };

            Supplier[] suppliers = importDtos
                .Select(dto => new Supplier()
                {
                    Name = dto.Name,
                    IsImporter = dto.IsImporter,
                })
                .ToArray();

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();


            return $"Successfully imported {suppliers.Length}";
        }

        //10.
        //public static string ImportParts(CarDealerContext context, string inputXml)
        //{
        //    XmlSerializer xmlSerializer = new XmlSerializer(typeof(PartsImportDto[]),
        //        new XmlRootAttribute("Parts"));

        //    PartsImportDto[] partsImportDtos;

        //    using (var reader = new StringReader(inputXml))
        //    { 
        //        partsImportDtos = (PartsImportDto[])xmlSerializer.Deserialize(reader);
        //    };

        //    var avalivableSupplier = context.Suppliers
        //        .Select(i => i.Id)
        //        .ToArray();




        //        return $"Successfully imported {cars.Count}";
        //}

    }
}