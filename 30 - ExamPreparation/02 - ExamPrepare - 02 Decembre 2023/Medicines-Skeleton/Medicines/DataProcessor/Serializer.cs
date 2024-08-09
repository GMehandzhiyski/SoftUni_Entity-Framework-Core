namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ExportDtos;

    public class Serializer
    {
        public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
        {
            DateTime dateDateTime;
             bool  isDateValid = DateTime
                .TryParse(date, out dateDateTime);  

            var allPatients = context.Patients
                .Where(p => p.PatientsMedicines.Any(p => p.Medicine.ProductionDate > dateDateTime))
                .Select(p => new ExportPatientXmlDto
                { 
                    Gender = p.Gender.ToString().ToLower(),
                    FullName = p.FullName,
                    AgeGroup = p.AgeGroup.ToString(),
                    Medicines = p.PatientsMedicines
                    .Where(p => p.Medicine.ProductionDate > dateDateTime)
                    .OrderByDescending(p => p.Medicine.ExpiryDate)
                    .ThenBy(p => p.Medicine.Price)
                    .Select(pc => new ExportMedicineXmlDto
                    { 
                        Category = pc.Medicine.Category.ToString().ToLower(),
                        Name = pc.Medicine.Name,
                        Price = pc.Medicine.Price.ToString("F2"),
                        Producer = pc.Medicine.Producer,
                        ExpiryDate = pc.Medicine.ExpiryDate.ToString("yyyy-MM-dd"),
                    })
                    .ToArray()
                })
                .OrderByDescending(p => p.Medicines.Length)
                .ThenBy(p => p.FullName)
                .ToArray();


          return XmlSerializationHelper
                .Serialize(allPatients, "Patients");
        }

        public static string ExportMedicinesFromDesiredCategoryInNonStopPharmacies(MedicinesContext context, int medicineCategory)
        {
            return "";
        }
    }
}
