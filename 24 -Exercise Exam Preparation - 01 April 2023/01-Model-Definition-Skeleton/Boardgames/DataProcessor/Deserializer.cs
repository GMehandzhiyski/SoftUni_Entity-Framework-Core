namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ImportDto;
    using Boardgames.Helpers;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            var creatorDtos = XmlSerializationHelper
                .Deserialize<ImportCreatorDto[]>(xmlString, "Creators");

            StringBuilder   sb = new StringBuilder();

            HashSet<Creator> creators = new HashSet<Creator>();

            foreach (var creatorDto in creatorDtos)
            {
                if (!IsValid(creatorDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Creator creator = new ()
                {
                    FirstName = creatorDto.FirstName,
                    LastName = creatorDto.LastName,
                };
 
                foreach (var boardDto in creatorDto.Boardgames) 
                {
                    if (!IsValid(boardDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                   creator.Boardgames.Add(new Boardgame()
                    {
                        Name = boardDto.Name,
                        Rating = boardDto.Rating,
                        YearPublished = boardDto.YearPublished,
                        CategoryType  = (CategoryType)boardDto.CategoryType,
                        Mechanics = boardDto.Mechanics
                    });
                }

                creators.Add(creator);

                sb.AppendLine(string.Format(SuccessfullyImportedCreator,
                                            creator.FirstName,
                                            creator.LastName,
                                            creator.Boardgames.Count()));
            }

            context.Creators.AddRange(creators);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            List<Seller> sellers = new List<Seller>();

            var sellersDtos = JsonConvert.
                DeserializeObject<ImportSellerDto[]>(jsonString);

            var uniqueBoardIds = context.Boardgames
                .Select(x => x.Id)
                .ToArray();

            foreach ( var sellerDto in sellersDtos)
            {
                if (!IsValid(sellerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Seller seller = new Seller()
                {
                   Name = sellerDto.Name,
                   Address = sellerDto.Address,
                   Country = sellerDto.Country,
                   Website  = sellerDto.Website,
                };

                foreach (var boardDto in sellerDto.BoardgamesIds.Distinct())
                {
                    if (!uniqueBoardIds.Contains(boardDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                   BoardgameSeller boardgame = new BoardgameSeller()
                    {
                       Seller = seller,
                       BoardgameId = boardDto
                   };

                    seller.BoardgamesSellers.Add(boardgame);
                  
                }

                sellers.Add(seller);
                sb.AppendLine(string.Format(SuccessfullyImportedSeller,
                                           seller.Name,
                                           seller.BoardgamesSellers.Count()));

            }
            context.Sellers.AddRange(sellers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
