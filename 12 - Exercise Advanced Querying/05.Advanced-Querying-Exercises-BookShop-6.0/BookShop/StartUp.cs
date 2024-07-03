namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

    public class StartUp
    {
        public static void Main()
        {
            using var context = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            Console.WriteLine(GetBooksByAgeRestriction(context, "teEN"));
        }

        //02.
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            if(!Enum.TryParse(command, true, out AgeRestriction ageRestrivtion))
            {
                return string.Empty;
            }

            //var commandToLower = command.ToLower();
            var books = context.Books
                .Where(b => b.AgeRestriction == ageRestrivtion)
                .Select(b => b.Title)
                .OrderBy(b => b);
                //.ToList();


            return string.Join(Environment.NewLine,books);
        }
    }
}


