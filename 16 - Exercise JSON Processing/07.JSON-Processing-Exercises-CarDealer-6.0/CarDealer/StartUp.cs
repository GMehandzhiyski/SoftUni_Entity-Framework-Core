﻿using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
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
            //Console.WriteLine(ImportSuppliers(context, userText));

            //10.
            //string partsString = File.ReadAllText("../../../Datasets/parts.json");
            //Console.WriteLine(ImportParts(context, partsString));

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
            //Console.WriteLine(GetOrderedCustomers(context));

            //15.
            //Console.WriteLine(GetCarsFromMakeToyota(context));

            //16.
            //Console.WriteLine(GetLocalSuppliers(context));

            //17.
            //Console.WriteLine(GetCarsWithTheirListOfParts(context));

            //18.
            //Console.WriteLine(GetTotalSalesByCustomer(context));

            //18.
            Console.WriteLine(GetSalesWithAppliedDiscount(context));
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
                .ToArray();

            var partsImport = parts
                .Where(p => validSupplierId.Contains(p.SupplierId))
                .ToArray();

            context.Parts.AddRange(partsImport);
            context.SaveChanges();

            return $"Successfully imported {partsImport.Length}.";
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

        //15.
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var toyotaCars = context.Cars
                .Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .Select(c => new
                {
                    c.Id,
                    c.Make,
                    c.Model,
                    c.TraveledDistance
                });


            var json = JsonConvert.SerializeObject(toyotaCars, Formatting.Indented);
            return json;
        }

        //16.
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(c => c.IsImporter == false)
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    PartsCount = c.Parts.Count()
                });


            var jason = JsonConvert.SerializeObject(suppliers, Formatting.Indented);
            return jason;
        }

        //17.
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var allCars = context.Cars
                .Select(c => new
                {
                    car = new
                    {
                        c.Make,
                        c.Model,
                        c.TraveledDistance
                    },

                    parts = c.PartsCars.Select(pc => new
                    {
                        pc.Part.Name,
                        Price = pc.Part.Price.ToString("f2")
                    })
                });



            var json = JsonConvert.SerializeObject(allCars, Formatting.Indented);
            return json;

        }


        //18.
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var allCustomers = context.Customers
                .Where(s => s.Sales.Any())
                .Select(s => new
                {
                    fullName = s.Name,
                    boughtCars = s.Sales.Count(),
                    spentMoney = s.Sales
                                .SelectMany(s => s.Car.PartsCars
                                                      .Select(pc => pc.Part.Price)).Sum()
                                                      //.ToString("f2")

                })
                .OrderByDescending(s => s.spentMoney)
                .ThenByDescending(s => s.boughtCars)
                .ToArray();

            var json = JsonConvert.SerializeObject(allCustomers, Formatting.Indented);
            return json;
        }

        //19.
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales10 = context.Sales
                .Take(10)
                .Select(s => new
                {
                    car = new
                    {
                        s.Car.Make,
                        s.Car.Model,
                        s.Car.TraveledDistance
                    },

                    customerName = s.Customer.Name,
                    discount = s.Discount.ToString("f2"),
                    price = s.Car.PartsCars.Select(p => p.Part.Price).Sum().ToString("f2"),
                    priceWithDiscount = (s.Car.PartsCars.Select(p => p.Part.Price).Sum() * (1 - s.Discount/100)).ToString("f2")

                });
           

            var json = JsonConvert.SerializeObject(sales10, Formatting.Indented);
            return json;
        }
    }

}