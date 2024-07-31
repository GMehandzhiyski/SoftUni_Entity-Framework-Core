namespace Invoices.DataProcessor
{
    //using Boardgames.Helpers;
    using Invoices.Data;
    using Invoices.DataProcessor.ExportDto;
    using Invoices.DataProcessor.ImportDto;
    using Newtonsoft.Json;
    using System.Data.SqlTypes;
    using System.Globalization;

    public class Serializer
    {
        public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
        {
            //XmlSerializationHelper xmlHelper = new XmlSerializationHelper();

           ExportClientDto[] clientsToExport = context.Clients
                .Where(cl => cl.Invoices.Any(i => i.IssueDate > date))
                .Select(cl => new ExportClientDto()
                {
                    InvoicesCount = cl.Invoices.Count,
                    ClientName = cl.Name,
                    VatNumber = cl.NumberVat,
                    Invoices = cl.Invoices
                        .OrderBy(i => i.IssueDate)
                        .ThenByDescending(i => i.DueDate)
                        .Select(i => new ExportInvoiceDto()
                        { 
                            InvoiceNumber = i.Number,
                            InvoiceAmount = i.Amount,
                            DueDate = i.DueDate.ToString("d", CultureInfo.InvariantCulture),
                            Currency = i.CurrencyType.ToString(),
                        })
                        .ToArray()


                })
                .OrderByDescending(i => i.InvoicesCount)
                .ThenBy(i => i.ClientName)
                .ToArray();

            return XmlSerializationHelper.Serialize(clientsToExport, "Clients");
        }

        public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
        {
            var clientsToExport = context.Products
                .Where(p => p.ProductsClients.Any())
                .Where(p => p.ProductsClients.Any(pc => pc.Client.Name.Length >= nameLength))
                .Select(p => new ExportProductDto()
                {
                    Name = p.Name,
                    Price = p.Price,
                    Category = p.CategoryType.ToString(),
                    Clients = p.ProductsClients
                        .Where(pc => pc.Client.Name.Length >= nameLength)
                        .Select(pc => new ExportJsonClientDto()
                        { 
                            Name = pc.Client.Name,
                            NumberVat = pc.Client.NumberVat
                        
                        })
                        .OrderBy(c => c.Name)
                        .ToArray()

                })
                .OrderByDescending(p => p.Clients.Length)
                .ThenBy(p => p.Name)
                .Take(5)
                .ToArray();

            return  JsonConvert.SerializeObject(clientsToExport, Formatting.Indented);
        }
    }
}