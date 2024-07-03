namespace BookShop
{
    using Data;
    using Initializer;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

    public class StartUp
    {
        public static void Main()
        {
            using var context = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            Console.WriteLine(GetBooksByAgeRestriction(context, "miNor"));
        }

        //01.
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var commandToLower = command.ToLower();
            var books = context.Books
                .Where(b => b.Title.ToLower() == commandToLower)
                .Select(b => b.Title)
                .OrderBy(b => b);


            return string.Join(Environment.NewLine,books);
        }
    }
}


