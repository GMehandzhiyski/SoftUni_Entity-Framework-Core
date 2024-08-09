using NetPay.Data;
using NetPay.Data.Models;
using NetPay.DataProcessor.ImportDtos;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetPay.DataProcessor
{
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data format!";
        private const string DuplicationDataMessage = "Error! Data duplicated.";
        private const string SuccessfullyImportedHousehold = "Successfully imported household. Contact person: {0}";
        private const string SuccessfullyImportedExpense = "Successfully imported expense. {0}, Amount: {1}";

        public static string ImportHouseholds(NetPayContext context, string xmlString)
        {
           StringBuilder sb = new StringBuilder();

            var householdsDeserializer = XmlSerializationHelper
                .Deserialize<ImportHouseHoldXmlDto[]>(xmlString, "Households");

            HashSet<Household> houses = new HashSet<Household>();   

            foreach (var houseDto in householdsDeserializer)
            {
                if (!IsValid(houseDto))
                { 
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (houses.FirstOrDefault(n => n.ContactPerson == houseDto.ContactPerson) != null)
                {
                    sb.AppendLine(DuplicationDataMessage);
                    continue;
                }

                if (houses.FirstOrDefault(n => n.Email == houseDto.Email) != null)
                {
                    sb.AppendLine(DuplicationDataMessage);
                    continue;
                }

                if (houses.FirstOrDefault(n => n.PhoneNumber == houseDto.PhoneNumber) != null)
                {
                    sb.AppendLine(DuplicationDataMessage);
                    continue;
                }

                Household newHouseHold = new Household()
                {
                    PhoneNumber = houseDto.PhoneNumber,
                    ContactPerson = houseDto.ContactPerson,
                    Email = houseDto.Email,
                
                };   

                houses.Add(newHouseHold);
                sb.AppendLine(string.Format(SuccessfullyImportedHousehold, newHouseHold.ContactPerson));
            }

            context.Households.AddRange(houses);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportExpenses(NetPayContext context, string jsonString)
        {
            return "";
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            foreach(var result in validationResults)
            {
                string currvValidationMessage = result.ErrorMessage;
            }

            return isValid;
        }
    }
}
