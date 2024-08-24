using NetPay.Data;
using NetPay.DataProcessor.ExportDtos;
using Newtonsoft.Json;

namespace NetPay.DataProcessor
{
    public class Serializer
    {
        public static string ExportHouseholdsWhichHaveExpensesToPay(NetPayContext context)
        {
            var  allHouseholds = context.Households
                .Where(h => h.Expenses.Any(p => p.PaymentStatus != Data.Models.Enums.PaymentStatus.Paid))
                .Select(h => new ExportHouseholdXmlDto 
                {   
                    ContactPerson = h.ContactPerson,
                    Email = h.Email,
                    PhoneNumber = h.PhoneNumber,    
                    Expenses = h.Expenses
                    .Where(h => h.PaymentStatus != Data.Models.Enums.PaymentStatus.Paid)
                    .Select(h => new ExportExpenseXmlDto 
                    {
                        ExpenseName = h.ExpenseName,
                        Amount = h.Amount.ToString("f2"),
                        DueDate = h.DueDate.ToString("yyyy-MM-dd"),
                        ServiceName = h.Service.ServiceName
                    
                    })
                    .OrderBy(h => h.DueDate)
                    .ThenBy(h => h.Amount)
                    .ToArray()
                
                })
                .OrderBy(h => h.ContactPerson)
                .ToArray();

            return XmlSerializationHelper.Serialize(allHouseholds, "Households");
        }

        public static string ExportAllServicesWithSuppliers(NetPayContext context)
        {
            var allservice = context.Services
                .OrderBy(s => s.ServiceName)
                .Select(s => new ExportServicedDto
                {
                    ServiceName = s.ServiceName,
                   Suppliers = s.SuppliersServices
                   .Select(s => new ExportSuppliersDto 
                   { 
                       SupplierName = s.Supplier.SupplierName,
                   
                   })
                   .OrderBy(s => s.SupplierName)
                   .ToArray()

                })
                .ToArray();

            return JsonConvert.SerializeObject(allservice, Formatting.Indented);
        }
    }
}
