﻿using AutoMapper.Configuration.Annotations;
using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Castle.Core.Resource;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();

            //09.
            //string userXml = File.ReadAllText("../../../Datasets/suppliers.xml");
            //Console.WriteLine(ImportSuppliers(context, userXml));

            //10.
            //string userXml = File.ReadAllText("../../../Datasets/parts.xml");
            //Console.WriteLine(ImportParts(context, userXml));

            //11.
            //string userXml = File.ReadAllText("../../../Datasets/cars.xml");
            //Console.WriteLine(ImportCars(context, userXml));

            //12.
            //string customersXml = File.ReadAllText("../../../Datasets/customers.xml");
            //Console.WriteLine(ImportCustomers(context, customersXml));

            //13.
            //string customersXml = File.ReadAllText("../../../Datasets/sales.xml");
            //Console.WriteLine(ImportSales(context, customersXml));

            //14.
            Console.WriteLine(GetCarsWithDistance(context));
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
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PartsImportDto[]),
                new XmlRootAttribute("Parts"));

            PartsImportDto[] partsImportDtos;

            using (var reader = new StringReader(inputXml))
            {
                partsImportDtos = (PartsImportDto[])xmlSerializer.Deserialize(reader);
            };

            var avalivableSupplier = context.Suppliers
                .Select(i => i.Id)
                .ToArray();

            var addNewParts = partsImportDtos
                .Where(p => avalivableSupplier.Contains(p.SupplierId));

            var parts = addNewParts
                .Select(dto => new Part()
                {
                    Name = dto.Name,
                    Price = dto.Price,
                    Quantity = dto.Quantity,
                    SupplierId =dto.SupplierId
                })
                .ToList();

            context.Parts.AddRange(parts);
            context.SaveChanges();


            return $"Successfully imported {parts.Count}";
        }

        //11.
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CarImportDto[]),
                new XmlRootAttribute("Cars"));

            CarImportDto[] carImportDtos;
            using (StringReader reader = new StringReader(inputXml))
            {
                carImportDtos = (CarImportDto[])xmlSerializer.Deserialize(reader);
            };

            List<Car> cars = new List<Car>();

            foreach (var dto in carImportDtos)
            {
                Car car = new Car()
                {
                    Make = dto.Make,
                    Model = dto.Model,
                    TraveledDistance = dto.TraveledDistance
                };

                int[] carPartsId = dto.PartIds
                    .Select(p => p.Id)
                    .Distinct()
                    .ToArray();

                var carParts = new List<PartCar>();

                foreach (var id in carPartsId)
                {
                    carParts.Add(new PartCar()
                    {
                        Car = car,
                        PartId = id
                    });
                }

                car.PartsCars = carParts;
                cars.Add(car);
            }
            context.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        //12.
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CustomerImportDto[]),
                new XmlRootAttribute("Customers"));

            CustomerImportDto[] customerImportDtos;
            using (StringReader reader = new StringReader(inputXml))
            {
                customerImportDtos = (CustomerImportDto[])xmlSerializer.Deserialize(reader);
            };

            Customer[] customers = customerImportDtos
                .Select(dto => new Customer()
                {
                    Name = dto.Name,
                    BirthDate = dto.BirthDate,
                    IsYoungDriver = dto.IsYoungDriver
                }).ToArray();

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Length}";
        }

        //13.
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SaleImportDto[]),
                new XmlRootAttribute("Sales"));

            using StringReader reader = new StringReader(inputXml);
            SaleImportDto[] saleImportDtos = (SaleImportDto[])xmlSerializer.Deserialize(reader);


            var validCarsId = context.Cars
                .Select(c => c.Id)
                .ToArray();

            var totalCars = saleImportDtos
                .Where(s => validCarsId.Contains(s.CarId))
                .ToList();

            var sales = totalCars
                .Select(s => new Sale()
                {
                    CarId = s.CarId,
                    CustomerId = s.CustomerId,
                    Discount = s.Discount,

                })
                .ToArray();

           context.Sales.AddRange(sales);
           context.SaveChanges();

            return $"Successfully imported {sales.Length}";
        }


        //14.
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var carWithDistance = context.Cars
                .Where(c => c.TraveledDistance > 2_000_000)
                .Select(c => new CarWithDistanceExportDto()
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance
                })
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ToArray();

            return SerializeToXml(carWithDistance, "cars");
        }



        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dto"></param>
        /// <param name="xmlRootAttribute"></param>
        /// <param name="omitDeclaration"></param>
        /// <returns></returns>
        private static string SerializeToXml<T>(T dto, string xmlRootAttribute, bool omitDeclaration = false)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRootAttribute));
            StringBuilder stringBuilder = new StringBuilder();

            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = omitDeclaration,
                Encoding = new UTF8Encoding(false),
                Indent = true
            };

            using (StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture))
            using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
            {
                XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
                xmlSerializerNamespaces.Add(string.Empty, string.Empty);

                try
                {
                    xmlSerializer.Serialize(xmlWriter, dto, xmlSerializerNamespaces);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return stringBuilder.ToString();
        }
    }
}