namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ExportDtos;
    using Newtonsoft.Json;
    using System.Globalization;

    public class Serializer
    {
        public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
        {
            DateTime dateDateTime = DateTime.Parse(date);

            var exportPatient = context.Patients
                .Where(p => p.PatientsMedicines.Any(m => m.Medicine.ProductionDate >= dateDateTime))
                .Select(p => new ExportPatientDto()
                {
                    Gender = p.Gender.ToString().ToLower(),
                    FullName = p.FullName,
                    AgeGroup = p.AgeGroup,
                    Medicines = p.PatientsMedicines
                    .Where(pm => pm.Medicine.ProductionDate >= dateDateTime)
                    .OrderByDescending(m => m.Medicine.ExpiryDate)
                    .ThenBy(m => m.Medicine.Price)
                        .Select(m => new ExportMedicineDto()
                        { 
                            Category = m.Medicine.Category.ToString().ToLower(),
                            Name = m.Medicine.Name,
                            Price = m.Medicine.Price.ToString("F2"),
                            Producer = m.Medicine.Producer.ToString(),
                            BestBefore = m.Medicine.ExpiryDate.ToString("yyyy-MM-dd")
                        })
                        .ToArray()
                })
                .OrderByDescending(p => p.Medicines.Length)
                .ThenBy(p => p.FullName)
                .ToArray();


            return XmlSerializationHelper.Serialize(exportPatient, "Patients");
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
