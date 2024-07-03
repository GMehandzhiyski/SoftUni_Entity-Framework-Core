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

            Console.WriteLine(GetBooksNotReleasedIn(context, 2000));
        }

        //02.
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            if(!Enum.TryParse(command, true, out AgeRestriction ageRestriction))
            {
                return string.Empty;
            }

            //var commandToLower = command.ToLower();
            var books = context.Books
                .Where(b => b.AgeRestriction == ageRestriction)
                .Select(b => b.Title)
                .OrderBy(b => b);
                //.ToList();


            return string.Join(Environment.NewLine,books);
        }

        //03.
        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.EditionType == EditionType.Gold
                            && b.Copies < 5000)
                .Select(b => new
                {
                    b.Title,
                    b.BookId
                })
                .OrderBy(b => b.BookId);
                

            return string.Join(Environment.NewLine,books.Select(b => b.Title));
        }

        //04.
        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .OrderByDescending(b => b.Price)
                .ToList();


            return string.Join(Environment.NewLine,
                books.Select(a => $"{a.Title} - {a.Price:f2}" ));
        }

        //05.
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {

            return "";
        }
    }
}


