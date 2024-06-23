using SoftUni.Data;
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SoftUni.Models;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
           var context = new SoftUniContext();
            Console.WriteLine(GetEmployeesInPeriod(context));
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


        //06.
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            Address newAdress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            var nakov = context.Employees
                .FirstOrDefault(e => e.LastName == "Nakov");

            if (nakov != null) 
            {
                nakov.Address = newAdress;
                context.SaveChanges();
            }

            var employees = context.Employees
                .Select(e => new
                {
                    e.Address.AddressText,
                    e.AddressId
                })
                .OrderByDescending(e => e.AddressId)
                .Take(10);

            var sb = new StringBuilder();
            foreach (var e in employees) 
            {
                sb.AppendLine(e.AddressText);
            }

            return sb.ToString().TrimEnd();
        }

        //07.

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employees = context.Employees
                .Select(e => new
                {
                    EmpName = $"{ e.FirstName + " " + e.LastName}",
                    ManagerName = $"{e.Manager.FirstName + " " + e.Manager.LastName}",
                    Projects = e.EmployeesProjects
                        .Where(ep => ep.Project.StartDate.Year >= 2001
                                      && ep.Project.StartDate.Year <= 2003)
                        .Select(ep => new 
                        {
                             ProjectName = ep.Project.Name,
                                           ep.Project.StartDate,
                                           EndDate =  ep.Project.EndDate.HasValue ?
                                                      ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt") :
                                                      "not finished"
                        })

                })
                .Take(10)
                .ToList();

            var sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.EmpName} - Manager: {e.ManagerName}");
                foreach (var p in e.Projects)
                { 
                    sb.AppendLine($"--{p.ProjectName} - {p.StartDate:M/d/yyyy h:mm:ss tt} - {p.EndDate}");
                
                }
            }
            return sb.ToString().TrimEnd();

        
        }

    }

    
}
