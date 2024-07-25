using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.Diagnostics;
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
            string userXml = File.ReadAllText("../../../Datasets/products.xml");
            Console.WriteLine(ImportProducts(context, userXml));

            //03.
            // string userXml = File.ReadAllText("../../../Datasets/categories.xml");
            //Console.WriteLine(ImportCategories(context, userXml));
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
                .Where(i => i.BuyerId != 0 && i.BuyerId != null)
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
    }



}