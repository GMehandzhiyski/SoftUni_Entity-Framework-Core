using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();

            //01.
            //string userXml = File.ReadAllText("../../../Datasets/users.xml");
            //Console.WriteLine(ImportUsers(context, userXml));

            //02.
            //string userXml = File.ReadAllText("../../../Datasets/products.xml");
            //Console.WriteLine(ImportProducts(context, userXml));

            //03.
            //string userXml = File.ReadAllText("../../../Datasets/categories.xml");
            //Console.WriteLine(ImportCategories(context, userXml));

            //04.
            //string userXml = File.ReadAllText("../../../Datasets/categories-products.xml");
            //Console.WriteLine(ImportCategoryProducts(context, userXml));

            //05.
           // Console.WriteLine(GetProductsInRange(context));

            //06.
            Console.WriteLine(GetSoldProducts(context));
        }

        //01.
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportUsersDto[]),
                new XmlRootAttribute("Users"));

            ImportUsersDto[] importUsersDots;
            using (var reader = new StringReader(inputXml))
            {
                importUsersDots = (ImportUsersDto[])xmlSerializer.Deserialize(reader);
            };

            var users = importUsersDots
                .Select(i => new User()
                {
                    FirstName = i.FirstName,
                    LastName = i.LastName,
                    Age = i.Age,
                })
                .ToArray();

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
        }

        //02.
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportProductsDto[]),
                new XmlRootAttribute("Products"));

            ImportProductsDto[] importProductsDots;
            using (var reader = new StringReader(inputXml))
            {
                importProductsDots = (ImportProductsDto[])xmlSerializer.Deserialize(reader);
            };

            HashSet<int> uniqueBuyerIds = new HashSet<int>();

            var products = importProductsDots
               .Where(i => i.BuyerId is not null)
               //.Where(i => uniqueBuyerIds.Add((int)i.BuyerId))
                .Select(i => new Product()
                {
                    Name = i.Name,
                    Price = i.Price,
                    SellerId = i.SellerId,
                    BuyerId = i.BuyerId
                })
                .ToArray();

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }

        //03.
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCategoriesDto[]),
                new XmlRootAttribute("Categories"));

            ImportCategoriesDto[] importCategoriesDots;
            using (var reader = new StringReader(inputXml))
            {
                importCategoriesDots = (ImportCategoriesDto[])xmlSerializer.Deserialize(reader);
            };

            var categories = importCategoriesDots
                .Where(i => i.Name != null)
                .Select(i => new Category()
                {
                    Name = i.Name,
                })
                .ToList();

            context.Categories.AddRange(categories);
            context.SaveChanges();


            return $"Successfully imported {categories.Count}";
        }

        //04.
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCategoryProductDto[]),
                new XmlRootAttribute("CategoryProducts"));

            ImportCategoryProductDto[] importCategories;
            using (var reader = new StringReader(inputXml))
            {
                importCategories = (ImportCategoryProductDto[])xmlSerializer.Deserialize(reader);
            };

            //Console.WriteLine($"Imported CategoryProducts: {importCategories.Length}");

         
            //foreach (var category in importCategories.Take(5))
            //{
            //    Console.WriteLine($"Category ID: {category.CategoryId}, Product ID: {category.ProductId}");
            //}
            var validCategoryId = context.Categories
                .Select(v => v.Id)
                .ToHashSet();

            var validProductId = context.Products
                .Select(v => v.Id)
                .ToHashSet();

            //Console.WriteLine($"Valid Category IDs: {validCategoryId.Count}");
            //Console.WriteLine($"Valid Product IDs: {validProductId.Count}");

            var categoryProducts = importCategories
               .Where(vc => validCategoryId.Contains(vc.CategoryId))
               .Where(vp => validProductId.Contains(vp.ProductId))
                .Select(v => new CategoryProduct()
                { 
                    CategoryId = v.CategoryId,
                    ProductId = v.ProductId
                })
                .ToArray();

            //Console.WriteLine($"Filtered CategoryProducts: {categoryProducts.Length}");

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();  

            return $"Successfully imported {categoryProducts.Length}";
        }

        //05.
        public static string GetProductsInRange(ProductShopContext context)
        {
            var allproducts = context.Products
                .Where(p => p.Price >= 500
                            && p.Price <= 1000)
               .OrderBy(p => p.Price)
               .Select(p => new ExportProductInRangeDto()
               {
                   Name = p.Name,
                   Price = p.Price,
                   BuyerName = p.Buyer.FirstName + " " + p.Buyer.LastName,

               })
               .Take(10)
               .ToArray();

            return SerializeToXml(allproducts, "Products");
        }

        //06.
        public static string GetSoldProducts(ProductShopContext context)
        {
            var allUsers = context.Users
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Where(u => u.ProductsSold.Any())
                .Select(u => new ExportSoldProductsDto()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold
                               .Select(p => new ProductsDto
                               {
                                    Name = p.Name,
                                    Price = p.Price
                               })
                                .ToArray(),
                })
                .Take(5)
                .ToArray();

            return SerializeToXml(allUsers, "Users");
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