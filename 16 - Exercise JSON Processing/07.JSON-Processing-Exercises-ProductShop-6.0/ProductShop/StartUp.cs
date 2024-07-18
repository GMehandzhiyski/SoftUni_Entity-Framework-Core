using Newtonsoft.Json;
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
            string userText = File.ReadAllText("../../../Datasets/users.json");
            Console.WriteLine(ImportUsers(context,userText));    

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


    }
}