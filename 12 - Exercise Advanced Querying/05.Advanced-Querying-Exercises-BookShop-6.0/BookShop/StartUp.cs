namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using System.Globalization;
    using System.Text;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

    public class StartUp
    {
        public static void Main()
        {
            using var context = new BookShopContext();
            //DbInitializer.ResetDatabase(context);

            Console.WriteLine(RemoveBooks(context));
            //IncreasePrices(context);
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
            var books = context.Books
                .Where(b => b.ReleaseDate.HasValue 
                        && b.ReleaseDate.Value.Year != year)
                .Select(b => b.Title)
                .ToList();
            return string.Join(Environment.NewLine,books);
        }

        //06.
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] inputString = input
                .ToLower()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var books = context.BooksCategories
                .Where(bc => inputString.Contains(bc.Category.Name.ToLower()))
                .Select(bc => bc.Book.Title)
                .OrderBy(t => t)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        //07.
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime parseDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                .Where(b => b.ReleaseDate < parseDate)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new 
                {
                    b.Title,
                    b.EditionType,
                    b.ReleaseDate,
                    b.Price
                })
                .ToArray();

            return string.Join(Environment.NewLine, books.Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:f2}"));
        }

        //08.
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {


            var authors = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .ToArray()
                .Select(a => $"{a.FirstName} {a.LastName}")
                .OrderBy(n => n);
            return string.Join(Environment.NewLine, authors);
        }

        //09.
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var inputString = input.ToLower();

            var books = context.Books
                .Where(a => a.Title.ToLower().Contains(inputString))
                .Select(a => a.Title)
                .OrderBy(n => n);
            return string.Join(Environment.NewLine, books);
        }

        //10.
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .Select(b => new
                {
                    b.Title,
                    b.BookId,
                    b.Author.FirstName,
                    b.Author.LastName,
                })
                .OrderBy(n => n.BookId);
            return string.Join(Environment.NewLine, books.Select(a => $"{a.Title} ({a.FirstName} {a.LastName})"));
        }

        //11.
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
           
            return context.Books
                   .Count(b => b.Title.Length > lengthCheck);
        }


        //12.
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var books = context.Authors
                .Select(a => new
                {
                    a.FirstName,
                    a.LastName,
                    Copies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(n => n.Copies);
            return string.Join(Environment.NewLine, books.Select(a => $"{a.FirstName} {a.LastName} - {a.Copies}") );
        }

        //13.
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var books = context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    Profit = c.CategoryBooks.Sum(cb => cb.Book.Price * cb.Book.Copies)
                })
                .OrderByDescending(n => n.Profit)
                .ThenBy(n => n.CategoryName)
                .ToArray();

            return string.Join(Environment.NewLine, books.Select(a => $"{a.CategoryName} ${a.Profit:f2}")) ;
        }

        //14.
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var books = context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    MostBook = c.CategoryBooks
                        .OrderByDescending(b => b.Book.ReleaseDate)
                    .Select(b => $"{b.Book.Title} ({b.Book.ReleaseDate.Value.Year})")
                    .Take(3)
                })
                .OrderBy(n => n.CategoryName)
                .ToList();

          var sb = new StringBuilder();

            foreach (var cat in books)
            { 
                sb.AppendLine($"--{cat.CategoryName}");

                foreach (var item in cat.MostBook)
                {
                    sb.AppendLine(item);
                }

            
            }

            return sb.ToString().TrimEnd();
        }

        //15.
        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                //.Select(b => b.Price)
                .ToList();

            foreach (var book in books)
            {
                book.Price = book.Price + 5;
            }

            context.SaveChanges();
        }

        //16.
        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Copies < 4200)
                .ToArray();

            context.RemoveRange(books);
            context.SaveChanges();

            return books.Length;
        }

    }
}


