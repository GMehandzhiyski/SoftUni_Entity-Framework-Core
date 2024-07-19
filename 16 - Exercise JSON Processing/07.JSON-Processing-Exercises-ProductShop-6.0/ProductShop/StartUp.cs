using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;
using System.Collections.Specialized;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
           ProductShopContext  context = new ProductShopContext();


            //01.
            //string userText = File.ReadAllText("../../../Datasets/users.json");
            //Console.WriteLine(ImportUsers(context, userText));

            ////02.
            //string userText = File.ReadAllText("../../../Datasets/products.json");
            //Console.WriteLine(ImportProducts(context, userText));

            //03.
            //string userText = File.ReadAllText("../../../Datasets/categories.json");
            //Console.WriteLine(ImportCategories(context, userText));

            //04.
            //string userText = File.ReadAllText("../../../Datasets/categories-products.json");
            //Console.WriteLine(ImportCategoryProducts(context, userText));

            ////05.
            //Console.WriteLine(GetProductsInRange(context));

            //06.
            //Console.WriteLine(GetSoldProducts(context));

            //07.
            //Console.WriteLine(GetCategoriesByProductsCount(context));

            //08.
            Console.WriteLine(GetUsersWithProducts(context));
        }

        //01.
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<List<User>>(inputJson);

            context.Users.AddRange(users);
            context.SaveChanges();


            return $"Successfully imported {users.Count}";
        }

        //02.
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<List<Product>>(inputJson);

            if(products is not null)
            {
                context.Products.AddRange(products);
                context.SaveChanges();
            }


            return $"Successfully imported {products?.Count}";
        }

        //03.
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<List<Category>>(inputJson);

            var finalCategoties = categories?
                .Where(c => c.Name is not null)
                .ToArray();
            if (finalCategoties is not null)
            {
                context.Categories.AddRange(finalCategoties);
                context.SaveChanges();  
            
                return $"Successfully imported {finalCategoties.Length}";
            }

            return $"Successfully imported 0";
        }


        //04.
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        { 
            var catProducts = JsonConvert.DeserializeObject<List<CategoryProduct>>(inputJson);

            var categoryProducts = catProducts
                .Where(c => c.CategoryId != null && c.ProductId != null)
                .ToList();
            context.CategoriesProducts.AddRange(categoryProducts);
            context.SaveChanges();

           return $"Successfully imported {categoryProducts.Count}";
        }

        //05.
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500
                            && p.Price <= 1000)
                .Select(p => new
                {
                    name = p.Name,
                    price = p.Price,
                    seller = $"{p.Seller.FirstName} {p.Seller.LastName}",
                })
                .OrderBy(p => p.price)
                .ToList();

            var json = JsonConvert.SerializeObject(products, Formatting.Indented);

            return json;
        }

        //06.
        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                 .Where(u => u.ProductsSold.Any(u => u.BuyerId != null))
                 .Select(u => new
                 {
                     firstName = u.FirstName,
                     lastName = u.LastName,
                     soldProducts = u.ProductsSold
                     .Select(p => new
                     {
                         name = p.Name,
                         price = p.Price,
                         buyerFirstName = p.Buyer.FirstName,
                         buyerLastName = p.Buyer.LastName,

                     })
                     
                 })
                 .OrderBy(u => u.lastName)
                 .ThenBy(u => u.firstName);

         
            var json = JsonConvert.SerializeObject (users, Formatting.Indented);    
            return json;
        }

        //07.
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categorias = context.Categories
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoriesProducts.Count(),
                    averagePrice = c.CategoriesProducts
                                    .Average(cp => cp.Product.Price)
                                    .ToString("f2"),
                    totalRevenue = c.CategoriesProducts
                                    .Sum(cp => cp.Product.Price)
                                    .ToString("f2")

                })
                .OrderByDescending(c => c.productsCount);
            var json = JsonConvert.SerializeObject(categorias, Formatting.Indented);
            return json;
        }

        //08.
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var usersWithProduct = context.Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = u.ProductsSold
                        .Where(p => p.BuyerId != null)
                        .Select(p => new
                        {
                            name = p.Name,
                            price = p.Price
                        })
                        .ToArray()
                })
                .OrderByDescending(u => u.soldProducts.Count())
                .ToArray();


            var output = new
            {
                usersCount = usersWithProduct.Count(),
                users = usersWithProduct.Select(u => new
                {
                    u.firstName,
                    u.lastName,
                    u.age,
                    soldProducts = new
                    {
                        count = u.soldProducts.Count(),
                        products = u.soldProducts
                    }
                })
            };

            string json = JsonConvert.SerializeObject(output, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });

            return json;
        }
    }
}