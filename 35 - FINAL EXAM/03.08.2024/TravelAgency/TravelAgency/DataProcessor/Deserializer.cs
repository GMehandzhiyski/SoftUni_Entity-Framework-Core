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

            var importCustumers = XmlSerializationHelper
                .Deserialize<ImportCustomerXmlDto[]>(xmlString, "Customers");

            HashSet<Customer> customers = new HashSet<Customer>();

            foreach (var customerDto in importCustumers)
            {
                if (!IsValid(customerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (customers.Any(n => n.FullName == customerDto.FullName))
                {
                    sb.AppendLine(DuplicationDataMessage);
                    continue;
                }

                if (customers.Any(n => n.Email == customerDto.Email))
                {
                    sb.AppendLine(DuplicationDataMessage);
                    continue;
                }

                if (customers.Any(n => n.PhoneNumber == customerDto.phoneNumber))
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

            HashSet<Booking> bookings = new HashSet<Booking>();
            

            var boogingDeserialize =
              JsonConvert.DeserializeObject<ImportBookingDto[]>(jsonString);

            foreach (var bookDto in boogingDeserialize)
            {
                if (!IsValid(bookDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime BookingDateDateTime;
                bool isBookingDateValid = DateTime
                    .TryParseExact(bookDto.BookingDate, "yyyy-MM-dd", CultureInfo
                    .InvariantCulture, DateTimeStyles.None, out BookingDateDateTime);

 

                if (!isBookingDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Customer customer = context.Customers.FirstOrDefault(c => c.FullName == bookDto.CustomerName);

                if (customer == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

          

                TourPackage tourPackage = context.TourPackages.FirstOrDefault(t => t.PackageName == bookDto.TourPackageName);

                if (tourPackage == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (context.Bookings.Any(b => b.CustomerId == customer.Id
                                    && b.TourPackageId == tourPackage.Id))
                {
                    Booking bookingToUpdate = context.Bookings
                        .FirstOrDefault(b => b.CustomerId == customer.Id
                                    && b.TourPackageId == tourPackage.Id);
                    bookingToUpdate.TourPackageId = tourPackage.Id;
                    bookingToUpdate.CustomerId = customer.Id;
                    bookingToUpdate.BookingDate = BookingDateDateTime;
                    sb.AppendLine(String.Format(SuccessfullyImportedBooking, tourPackage.PackageName, BookingDateDateTime.ToString("yyyy-MM-dd")));
                    context.SaveChanges();
                    continue;
                }

                Booking newBooking = new Booking()
                {
                    BookingDate = BookingDateDateTime,
                    CustomerId = customer.Id,
                    TourPackageId = tourPackage.Id,
                };

                bookings.Add(newBooking);
                context.Bookings.Add(newBooking);
                context.SaveChanges();
                sb.AppendLine(String.Format(SuccessfullyImportedBooking, tourPackage.PackageName, BookingDateDateTime.ToString("yyyy-MM-dd")));
            }

          
           // context.Bookings.AddRange(bookings);
           // context.SaveChanges();

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
