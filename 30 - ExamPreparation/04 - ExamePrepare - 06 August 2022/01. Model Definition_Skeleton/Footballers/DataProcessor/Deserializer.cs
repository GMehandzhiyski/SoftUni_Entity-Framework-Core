namespace Footballers.DataProcessor
{
    using Footballers.Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            var coachesDeserializer = XmlSerializationHelper
                .Deserialize<ImportCoachXmlDto[]>(xmlString, "Coaches");

            HashSet<Coach> footballers = new HashSet<Coach>();    

            foreach (var coachDto in coachesDeserializer)
            {
                if (!IsValid(coachDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (string.IsNullOrEmpty(coachDto.Nationality))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Coach newCoach = new Coach()
                {
                    Name = coachDto.Name,
                    Nationality = coachDto.Nationality,
                };

                foreach (var footballerDto in coachDto.Footballers)
                {
                    if (!IsValid(footballerDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime contractStartDateDateTime;
                    bool isContractStartDateValid = DateTime
                        .TryParseExact(footballerDto.ContractStartDate, "dd/MM/yyyy", CultureInfo
                        .InvariantCulture, DateTimeStyles.None, out contractStartDateDateTime);

                    DateTime contractEndDateDateTime;
                    bool isContractEndDateValid = DateTime
                        .TryParseExact(footballerDto.ContractEndDate, "dd/MM/yyyy", CultureInfo
                        .InvariantCulture, DateTimeStyles.None, out contractEndDateDateTime);

                    if (!isContractStartDateValid
                        || !isContractEndDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (contractStartDateDateTime
                        >= contractEndDateDateTime)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    

                    Footballer footballer = new Footballer()
                    {
                        Name = footballerDto.Name,
                        ContractStartDate = contractStartDateDateTime,
                        ContractEndDate = contractEndDateDateTime,
                        BestSkillType = (BestSkillType)footballerDto.BestSkillType,
                        PositionType = (PositionType)footballerDto.PositionType

                    };

                    newCoach.Footballers.Add(footballer);
                }

                footballers.Add(newCoach);
                sb.AppendLine(string.Format(SuccessfullyImportedCoach, newCoach.Name, newCoach.Footballers.Count));
            }

            context.Coaches.AddRange(footballers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            StringBuilder   sb = new StringBuilder();

            var teamsDeserialize = JsonConvert
                .DeserializeObject<ImportTeamDto[]>(jsonString);

            HashSet<Team> teams = new HashSet<Team>();  

            foreach (var teamDto in teamsDeserialize)
            {
                if (!IsValid(teamDto)
                    || teamDto.Trophies == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Team newTeam = new Team()
                { 
                    Name = teamDto.Name,
                    Nationality = teamDto.Nationality,
                    Trophies    = teamDto.Trophies,

                };

                foreach (var footballDto in teamDto.Footballers.Distinct())
                {
                    if (context.Footballers.Find(footballDto) == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    };

                    TeamFootballer teamFootballer = new TeamFootballer()
                    { 
                        FootballerId = footballDto
                    };

                    newTeam.TeamsFootballers.Add(teamFootballer);
                }

                teams.Add(newTeam);
                sb.AppendLine(string.Format(SuccessfullyImportedTeam,
                    newTeam.Name,
                    newTeam.TeamsFootballers.Count()));
            }

            context.Teams.AddRange(teams);
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
