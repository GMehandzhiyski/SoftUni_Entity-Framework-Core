namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ExportDtos;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
        {
            return "";
        }

        public static string ExportMedicinesFromDesiredCategoryInNonStopPharmacies(MedicinesContext context, int medicineCategory)
        {


            var exportMedicine = context.Medicines
                .Where(m => m.Category == (Category)medicineCategory
                                            && m.Pharmacy.IsNonStop)
                .OrderBy(m => m.Price)
                .ThenBy(m => m.Name)
                .Select(m => new ExportMedicineJsonDto()
                {
                    Name = m.Name,
                    Price = m.Price.ToString("f2"),
                    Pharmacy = new ExportPharmacyJsonDto()
                        { 
                            Name = m.Pharmacy.Name,
                            PhoneNumber = m.Pharmacy.PhoneNumber
                        }
                })
                
                .ToArray();
               

               //(Category)Enum.Parse(typeof(Category), medicineCategory)
            return JsonConvert.SerializeObject(exportMedicine, Formatting.Indented);
        }
    }
}
