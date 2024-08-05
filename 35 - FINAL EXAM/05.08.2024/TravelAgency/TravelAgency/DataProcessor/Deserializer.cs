using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.DataProcessor.ImportDtos;

namespace TravelAgency.DataProcessor
{
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data format!";
        private const string DuplicationDataMessage = "Error! Data duplicated.";
        private const string SuccessfullyImportedCustomer = "Successfully imported customer - {0}";
        private const string SuccessfullyImportedBooking = "Successfully imported booking. TourPackage: {0}, Date: {1}";

        public static string ImportCustomers(TravelAgencyContext context, string xmlString)
        {
           StringBuilder sb = new StringBuilder();

            var customerDeserializer = XmlSerializationHelper
                .Deserialize<ImportCustormeDto[]>(xmlString, "Customers");

            HashSet<Customer> customers = new HashSet<Customer>();  

            foreach ( var customerDto in customerDeserializer ) 
            {
                if (!IsValid(customerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if ((customers.FirstOrDefault(f => f.FullName == customerDto.FullName) != null)
                    || (customers.FirstOrDefault(e => e.Email == customerDto.Email) != null)
                    || (customers.FirstOrDefault(p => p.PhoneNumber == customerDto.phoneNumber) != null ))
                {
                    sb.AppendLine(DuplicationDataMessage);
                    continue;
                }

                Customer newCustomer = new Customer()
                { 
                    PhoneNumber = customerDto.phoneNumber,
                    FullName = customerDto.FullName,
                    Email = customerDto.Email,  
                };

                customers.Add(newCustomer);
                sb.AppendLine(string.Format(SuccessfullyImportedCustomer, newCustomer.FullName));
                
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportBookings(TravelAgencyContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var bookingDeserializer = JsonConvert
                .DeserializeObject<ImportBookingDto[]>(jsonString);


            HashSet<Booking> bookings = new HashSet<Booking>();

            foreach (var bookingDto in bookingDeserializer)
            {
                if (!IsValid(bookingDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime bookingDateDateTime;
                bool isBookingDateValid = DateTime
                     .TryParseExact(bookingDto.BookingDate, "yyyy-MM-dd", CultureInfo
                     .InvariantCulture, DateTimeStyles.None, out bookingDateDateTime);

                if (!isBookingDateValid) 
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var currCustomer = context.Customers
                    .FirstOrDefault(c => c.FullName == bookingDto.CustomerName);
                var currPackedName = context.TourPackages
                    .FirstOrDefault(t => t.PackageName == bookingDto.TourPackageName);

                if (currCustomer == null
                    || currPackedName == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
         

                Booking newBooking = new Booking()
                {
                    CustomerId = currCustomer.Id,
                    TourPackageId = currPackedName.Id,
                    BookingDate = bookingDateDateTime,
                };

                bookings.Add(newBooking);
                sb.AppendLine(string.Format(SuccessfullyImportedBooking, bookingDto.TourPackageName, bookingDateDateTime.ToString("yyyy-MM-dd")));
            }

            context.Bookings.AddRange(bookings);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validateContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, validateContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                string currValidationMessage = validationResult.ErrorMessage;
            }

            return isValid;
        }
    }
}
