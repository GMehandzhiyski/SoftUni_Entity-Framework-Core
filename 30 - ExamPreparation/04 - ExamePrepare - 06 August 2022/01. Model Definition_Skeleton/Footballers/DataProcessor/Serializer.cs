namespace Footballers.DataProcessor
{
    using Data;
    using System.Text;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            StringBuilder sb = new StringBuilder();

            return sb.ToString().TrimEnd();
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            StringBuilder sb = new StringBuilder();

            return sb.ToString().TrimEnd();
        }
    }
}
