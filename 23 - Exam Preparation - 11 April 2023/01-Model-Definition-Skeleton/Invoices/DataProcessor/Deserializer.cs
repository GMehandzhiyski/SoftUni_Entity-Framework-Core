namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    //using Boardgames.Helpers;
    //using Castle.Components.DictionaryAdapter;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.Data.Models.Enums;
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
            ICollection<Invoice> invoices = new List<Invoice>();

            var deserialized = 
                JsonConvert.DeserializeObject<ImportInvoiceDto[]>(jsonString)!;

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

                Invoice newInvoice = new Invoice()
                {
                    Number = invoiceDto.Number,
                    IssueDate = issueDate,
                    DueDate = dueDate,
                    Amount = invoiceDto.Amount,
                    CurrencyType = (CurrencyType)invoiceDto.CurrencyType,
                    ClientId = invoiceDto.ClientId,
                };

                invoices.Add(newInvoice);
                sb.AppendLine(String.Format(SuccessfullyImportedInvoices, invoiceDto.Number));

            }

            context.Invoices.AddRange(invoices);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            HashSet<Product> products = new HashSet<Product>(); 

            var productDeserialize =
                JsonConvert.DeserializeObject < ImportProductsDto[]>(jsonString);

            foreach (var productDto in productDeserialize)
            {
                if (!IsValid(productDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Product newProduct = new Product()
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    CategoryType = (CategoryType)productDto.CategoryType,
                };

                HashSet<ProductClient> productClients = new HashSet<ProductClient>();

                foreach (var clientId in productDto.Clients.Distinct())
                {
                    if (!context.Clients.Any(c => c.Id == clientId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    ProductClient client = new ProductClient()
                    {
                        Product = newProduct,
                        ClientId = clientId,
                    };

                    productClients.Add(client);

                }

                newProduct.ProductsClients = productClients;

                products.Add(newProduct);
                sb.AppendLine(string.Format(SuccessfullyImportedProducts, productDto.Name, productClients.Count()));

            }

            context.Products.AddRange(products);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    } 
}
