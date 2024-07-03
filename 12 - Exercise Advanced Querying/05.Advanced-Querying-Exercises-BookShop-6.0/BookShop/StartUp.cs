﻿namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System.Globalization;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

    public class StartUp
    {
        public static void Main()
        {
            using var context = new BookShopContext();
            //DbInitializer.ResetDatabase(context);

            Console.WriteLine(GetAuthorNamesEndingIn(context, "e"));
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


    }
}


