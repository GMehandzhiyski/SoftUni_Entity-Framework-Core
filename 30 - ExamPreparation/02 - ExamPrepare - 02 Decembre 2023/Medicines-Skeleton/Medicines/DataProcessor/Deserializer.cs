namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ImportDtos;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data!";
        private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
        private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

        public static string ImportPatients(MedicinesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var patientsDeserializer = JsonConvert
                .DeserializeObject<ImportPacientsJsonDto[]>(jsonString);

            HashSet<Patient> patients = new HashSet<Patient>();

            foreach (var patientDto in patientsDeserializer)
            {
                if (!IsValid(patientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Patient newPatient = new Patient()
                { 
                    FullName = patientDto.FullName, 
                    AgeGroup = (AgeGroup)patientDto.AgeGroup,
                    Gender = (Gender)patientDto.Gender,
                };

                foreach (var medicineDto in patientDto.Medicines)
                {
                    if (!IsValid(medicineDto))
                    { 
                        sb.AppendLine(ErrorMessage);
                        continue; 
                    };

                    if (newPatient.PatientsMedicines.Any(m => m.MedicineId == medicineDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    };

                    PatientMedicine newPatientMedicine = new PatientMedicine() 
                    {
                        Patient = newPatient,
                        MedicineId = medicineDto
                    };

                    newPatient.PatientsMedicines.Add(newPatientMedicine);
                }

                patients.Add(newPatient);
                sb.AppendLine(string.Format(SuccessfullyImportedPatient, newPatient.FullName, newPatient.PatientsMedicines.Count()));
            }

            context.Patients.AddRange(patients);    
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPharmacies(MedicinesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            var pharmaciesDeserialize = XmlSerializationHelper
                .Deserialize<ImportPharmacyDto[]>(xmlString, "Pharmacies");

            HashSet<Pharmacy> pharmacies = new HashSet<Pharmacy>();

            foreach (var pharmacyDto in pharmaciesDeserialize)
            {
                if (!IsValid(pharmacyDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                
                Pharmacy newPharmacy = new Pharmacy()
                { 
                    IsNonStop = bool.Parse(pharmacyDto.IsNonStop),
                    Name = pharmacyDto.Name,
                    PhoneNumber = pharmacyDto.PhoneNumber,
                };

               // HashSet<Medicine> medicines = new HashSet<Medicine>();
                foreach (var medicineDto in pharmacyDto.Medicines)
                {
                    if (!IsValid(medicineDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    //DateTime dateTimeProductionDate = DateTime
                    //    .ParseExact(medicineDto.ProductionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                   DateTime dateTimeProductionDate;
                    bool isProductionDateValid = DateTime
                        .TryParseExact(medicineDto.ProductionDate, "yyyy-MM-dd", CultureInfo
                        .InvariantCulture, DateTimeStyles.None, out dateTimeProductionDate);

                    if (!isProductionDateValid)
                    {
                        sb.Append(ErrorMessage);
                        continue;
                    }

                  
                    DateTime dateTimeExpiryDate;
                    bool isExpityDateValid = DateTime
                        .TryParseExact(medicineDto.ExpiryDate, "yyyy-MM-dd", CultureInfo
                        .InvariantCulture, DateTimeStyles.None, out dateTimeExpiryDate);

                    if (!isExpityDateValid)
                    {
                        sb.Append(ErrorMessage);
                        continue;
                    }

                    if (dateTimeProductionDate >= dateTimeExpiryDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (newPharmacy.Medicines.Any(x => x.Name == medicineDto.Name 
                                                       && x.Producer == medicineDto.Producer))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                 

                    Medicine newMedicine = new Medicine()
                    {
                        Category = (Category)Enum.Parse(typeof(Category),medicineDto.Category),
                        Name = medicineDto.Name,
                        Price = (decimal)(medicineDto.Price),
                        ProductionDate = dateTimeProductionDate,
                        ExpiryDate = dateTimeExpiryDate,
                        Producer = medicineDto.Producer,
                    };

                   
                    newPharmacy.Medicines.Add(newMedicine);
                }

                

                pharmacies.Add(newPharmacy);
                sb.AppendLine(string
                   .Format(SuccessfullyImportedPharmacy,
                                    newPharmacy.Name,
                                    newPharmacy.Medicines.Count()));
            }

            context.Pharmacies.AddRange(pharmacies);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
