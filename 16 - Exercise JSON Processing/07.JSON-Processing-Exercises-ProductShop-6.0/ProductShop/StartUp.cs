﻿using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;

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
            Console.WriteLine(GetProductsInRange(context));
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


    }
}