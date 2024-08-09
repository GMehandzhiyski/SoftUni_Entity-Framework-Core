using NetPay.Data;
using NetPay.Data.Models;
using NetPay.Data.Models.Enums;
using NetPay.DataProcessor.ImportDtos;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
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
           StringBuilder sb = new StringBuilder();

            var expensesDes = JsonConvert
                .DeserializeObject<ImportExpenseDto[]>(jsonString);

            HashSet<Expense> expenses = new HashSet<Expense>(); 

            foreach (var expenseDto in expensesDes)
            {
                if (!IsValid(expenseDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;

                }

            

                DateTime dueDateTime;
                bool isDueDateTimeValid = DateTime
                     .TryParseExact(expenseDto.DueDate, "yyyy-MM-dd", CultureInfo
                     .InvariantCulture, DateTimeStyles.None, out dueDateTime);

                if (!isDueDateTimeValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;

                }

                if ((expenseDto.PaymentStatus != PaymentStatus.Unpaid.ToString()
                    && expenseDto.PaymentStatus != PaymentStatus.Paid.ToString()
                    && expenseDto.PaymentStatus != PaymentStatus.Expired.ToString()
                    && expenseDto.PaymentStatus != PaymentStatus.Overdue.ToString()))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var housId = context.Households
                   .FirstOrDefault(c => c.Id == expenseDto.HouseholdId);
                var ServiceId = context.Services
                    .FirstOrDefault(t => t.Id == expenseDto.ServiceId);

                if (housId == null
                   || ServiceId == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }


                Expense newExpense = new Expense() 
                {
                    ExpenseName = expenseDto.ExpenseName,
                    Amount =expenseDto.Amount,
                    DueDate= dueDateTime,
                    PaymentStatus = (PaymentStatus)Enum.Parse(typeof(PaymentStatus),expenseDto.PaymentStatus),
                    HouseholdId = expenseDto.HouseholdId,
                    ServiceId = expenseDto.ServiceId
                };

                expenses.Add(newExpense);
                sb.AppendLine(string.Format(SuccessfullyImportedExpense, newExpense.ExpenseName, newExpense.Amount.ToString("F2")));
            }

            context.Expenses.AddRange(expenses);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
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
