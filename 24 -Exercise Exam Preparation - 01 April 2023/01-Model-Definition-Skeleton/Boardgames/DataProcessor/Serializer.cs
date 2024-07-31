namespace Boardgames.DataProcessor
{
    using Boardgames.Data;
    using Boardgames.DataProcessor.ExportDto;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            var  allCreators = context.Creators
                .Where(c => c.Boardgames.Any())
                .Select(c => new ExportCreatorDto()
                { 
                    BoardgamesCount = c.Boardgames.Count(),
                    CreatorName = c.FirstName + " " + c.LastName,   
                    Boardgames = c.Boardgames
                    .Select(c => new ExportBoardgameXmlDto()
                    { 
                        BoardgameName = c.Name,
                        BoardgameYearPublished = c.YearPublished,
                    
                    })
                    .OrderBy(c => c.BoardgameName)
                    .ToArray()
                })
                .OrderByDescending(c => c.BoardgamesCount)
                .ThenBy(c => c.CreatorName)
                .ToArray();



            return XmlSerializationHelper.Serialize(allCreators, "Creators");
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {

            var topSellers = context.Sellers
                //.Where(s => s.BoardgamesSellers.Any())
                .Where(b => b.BoardgamesSellers.Any(bb => bb.Boardgame.YearPublished >= year
                                                          && bb.Boardgame.Rating <= rating))
                .Select(s => new ExportSellerDto()
                {
                    Name = s.Name,
                    Website = s.Website,
                    Boardgames = s.BoardgamesSellers
                        .Where(bb => bb.Boardgame.YearPublished >= year
                                        && bb.Boardgame.Rating <= rating)
                        .ToArray() 
                        .Select(b => new ExportBoardgameDto ()
                        {
                            Name = b.Boardgame.Name,
                            Rating = b.Boardgame.Rating,
                            Mechanics = b.Boardgame.Mechanics,
                            Category = b.Boardgame.CategoryType.ToString()
                        })
                        .OrderByDescending(b => b.Rating)
                        .ThenBy(b => b.Name)    
                        .ToArray()


                })
                .OrderByDescending(c => c.Boardgames.Count())
                .ThenBy (c => c.Name)
                .Take(5)
                .ToArray();

            return JsonConvert.SerializeObject(topSellers, Formatting.Indented);
        }
    }
}