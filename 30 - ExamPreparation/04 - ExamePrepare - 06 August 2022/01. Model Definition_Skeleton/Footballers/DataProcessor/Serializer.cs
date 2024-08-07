﻿namespace Footballers.DataProcessor
{
    using Data;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Text;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            var coachesSerializer = context.Coaches
                .Where(c => c.Footballers.Any())
                .Select(c => new ExportCoachXmlDto()
                {
                    FootballersCount = c.Footballers.Count(),
                    CoachName = c.Name,
                    Footballers = c.Footballers
                    .OrderBy(f => f.Name)
                    .Select(f => new ExportFootballerXmlDto()
                    {
                        Name = f.Name,
                        Position = f.PositionType.ToString()
                    })
                    .ToArray()
                })
                .OrderByDescending(c => c.FootballersCount)
                .ThenBy(c => c.CoachName)
                .ToArray();

            return XmlSerializationHelper.Serialize(coachesSerializer, "Coaches");
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            var teams = context.Teams
                .Where(t => t.TeamsFootballers.Any(f => f.Footballer.ContractStartDate >= date))
                .Select(t => new 
                { 
                    Name = t.Name,
                    Footballers = t.TeamsFootballers
                    .Where(f => f.Footballer.ContractStartDate >= date)
                    .OrderByDescending(f => f.Footballer.ContractEndDate)
                    .ThenBy(f => f.Footballer.Name)
                    .Select(f => new
                    { 
                        FootballerName = f.Footballer.Name,
                        ContractStartDate = f.Footballer.ContractStartDate.ToString("d",CultureInfo.InvariantCulture),
                        ContractEndDate = f.Footballer.ContractEndDate.ToString("d", CultureInfo.InvariantCulture),
                        BestSkillType = f.Footballer.BestSkillType.ToString(),
                        PositionType = f.Footballer.PositionType.ToString()    
                    })
                    .ToArray()
                })
                .OrderByDescending(t => t.Footballers.Length)
                .ThenBy(t => t.Name)
                .Take(5)
                .ToArray();

            return JsonConvert.SerializeObject(teams, Formatting.Indented);
        }
    }
}
