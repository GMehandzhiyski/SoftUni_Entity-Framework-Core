namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using Boardgames.Helpers;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";



        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            var creatorDtos = XmlSerializationHelper
                .Deserialize<ImportClientsDto[]>(xmlString, "Clients");

            StringBuilder sb = new();

            
            HashSet<Client> clients = new HashSet<Client>();

            foreach (var dtoClient in creatorDtos)
            {
                if (!IsValid(dtoClient))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                HashSet<Address> addresses = new HashSet<Address>();

                foreach (var dtoAddress in dtoClient.Address)
                {

                    if (!IsValid(dtoAddress))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Address newAddress = new Address()
                    {
                        StreetName = dtoAddress.StreetName,
                        StreetNumber = dtoAddress.StreetNumber,
                        PostCode = dtoAddress.PostCode,
                        City = dtoAddress.City,
                        Country = dtoAddress.Country,
                    };
                    addresses.Add(newAddress);
                }
                Client client = new Client()
                {
                    Name = dtoClient.Name,
                    NumberVat = dtoClient.NumberVat,
                    Addresses = addresses

                };

                clients.Add(client);
                sb.AppendLine(string.Format(SuccessfullyImportedClients, client.Name));



            }
            context.Clients.AddRange(clients);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }



        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            HashSet<Invoice> invoices = new HashSet<Invoice>();

            var deserialized = JsonConvert.DeserializeObject<ImportInvoiceDto[]>(jsonString)!;

            foreach (var invoiceDto in deserialized)
            {
                if (!IsValid(invoiceDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isIssueDateValid = DateTime
                    .TryParse(invoiceDto.IssueDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime issueDate);
                bool isDueDateValid = DateTime
                    .TryParse(invoiceDto.DueDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dueDate);

                if (isIssueDateValid == false
                    || isDueDateValid == false
                    || DateTime.Compare(dueDate, issueDate) < 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!context.Clients.Any(c => c.Id == invoiceDto.ClientId))
                {
                    sb.Append(ErrorMessage);
                    continue;
                }

                Invoice invoice = new Invoice()
                {
                    Number = invoiceDto.Number,
                    IssueDate = issueDate,
                    DueDate = dueDate,
                    Amount = invoiceDto.Amount,
                    CurrencyType = invoiceDto.CurrencyType,
                    ClientId = invoiceDto.ClientId,
                };
                invoices.Add(invoice);

            }

            context.Invoices.AddRange(invoices);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {


            throw new NotImplementedException();
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    } 
}
