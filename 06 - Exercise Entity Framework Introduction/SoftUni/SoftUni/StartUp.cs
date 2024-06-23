using SoftUni.Data;
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
           var context = new SoftUniContext();
            Console.WriteLine(GetEmployeesFromResearchAndDevelopment(context));
        }

        //03.
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context.Employees
                .Select(e => new
                 {
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary
                 })
                .ToList();

            var sb = new StringBuilder();

            foreach (var e in employees) 
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //04.
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {

            var richEmployee = context.Employees
                .Where(e => e.Salary > 50_000)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .OrderBy(e => e.FirstName);

              var sb = new StringBuilder();

            foreach (var e in richEmployee)
            {
                sb.AppendLine($"{e.FirstName} - {e.Salary:f2}");
            }


            return sb.ToString().TrimEnd();
        }


        //05.
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context.Employees
                 .Where(e => e.Department.Name == "Research and Development")
                 .Select(e => new
                 {
                     e.FirstName,
                     e.LastName,
                     e.Department,
                     e.Salary
                 })
                 .OrderBy(e => e.Salary)
                 .ThenByDescending(e => e.FirstName);

            var sb = new StringBuilder();
            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} from {e.Department.Name} - ${e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
