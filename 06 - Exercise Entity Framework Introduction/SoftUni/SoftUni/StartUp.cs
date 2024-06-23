using SoftUni.Data;
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SoftUni.Models;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System.Diagnostics.Metrics;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var context = new SoftUniContext();
            Console.WriteLine(DeleteProjectById(context));
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
                    e.Address!.AddressText,
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
                    EmpName = $"{e.FirstName + " " + e.LastName}",
                    ManagerName = $"{e.Manager!.FirstName + " " + e.Manager.LastName}",
                    Projects = e.EmployeesProjects
                        .Where(ep => ep.Project.StartDate.Year >= 2001
                                      && ep.Project.StartDate.Year <= 2003)
                        .Select(ep => new
                        {
                            ProjectName = ep.Project.Name,
                            ep.Project.StartDate,
                            EndDate = ep.Project.EndDate.HasValue ?
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

        //08.
        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses
                .OrderByDescending(a => a.Employees.Count())
                .ThenBy(a => a.Town!.Name)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .Select(a => new
                {
                    a.AddressText,
                    TownName = a.Town!.Name,
                    CountEmp = a.Employees.Count()
                })
                .ToList();


            var sb = new StringBuilder();

            foreach (var a in addresses)
            {
                sb.AppendLine($"{a.AddressText}, {a.TownName} - {a.CountEmp} employees");
            }

            return sb.ToString().TrimEnd();
        }


        //09.
        public static string GetEmployee147(SoftUniContext context)
        {
            var employees147 = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    CurrProject = e.EmployeesProjects
                                 .Select(ep => ep.Project.Name)
                                 .OrderBy(p => p)
                                 .ToList()
                })
                .FirstOrDefault();

            var sb = new StringBuilder();
            if (employees147 != null)
            {
                sb.AppendLine($"{employees147.FirstName} {employees147.LastName} - {employees147.JobTitle}");
                foreach (var cp in employees147.CurrProject)
                {
                    sb.AppendLine(cp);
                }

            }

            return sb.ToString().TrimEnd();
        }

        //10.
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var depart = context.Departments
                .Where(d => d.Employees.Count() > 5)
                .OrderBy(d => d)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    d.Name,
                    d.Manager.FirstName,
                    d.Manager.LastName,
                    TeamDepart = d.Employees
                                .Select(e => new
                                {
                                    e.FirstName,
                                    e.LastName,
                                    e.JobTitle,
                                })
                                .OrderBy(e => e.FirstName)
                                .ThenBy(e => e.LastName)
                                .ToList()
                })
                .ToList();

            var sb = new StringBuilder();
            foreach (var d in depart)
            {
                sb.AppendLine($"{d.Name} - {d.FirstName} {d.LastName}");
                foreach (var e in d.TeamDepart)
                { 
                    sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                }

            }
            return sb.ToString().TrimEnd();

        }

        //11.
        public static string GetLatestProjects(SoftUniContext context)
        {
            var project = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .OrderBy(p => p.Name)
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    p.StartDate
                });

            var sb = new StringBuilder();

            foreach (var p in project)
            {
                sb.AppendLine(p.Name);
                sb.AppendLine(p.Description);
                sb.AppendLine($"{p.StartDate:M/d/yyyy h:mm:ss tt}");
            }


            return sb.ToString().TrimEnd();
        }

        //12.
        public static string IncreaseSalaries(SoftUniContext context)
        {
            List<string> deptIncSalary = new List<string>
            {
                  "Engineering",
                  "Tool Design",
                  "Marketing",
                  "Information Services"
            };

            var employees = context.Employees
                .Where(e => deptIncSalary.Contains(e.Department.Name))
                .ToList();
            foreach (var e in employees)
            {
                e.Salary = e.Salary * 1.12m;
            }
            context.SaveChanges();

            var finalEmployees = employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            var sb = new StringBuilder();

            foreach (var e in finalEmployees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:f2})");
            }
            return sb.ToString().TrimEnd();
        }


        //13.
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.FirstName.ToLower().StartsWith("sa"))
                .Select(e => new 
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            var sb = new StringBuilder();
            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})");
            }
            return sb.ToString().TrimEnd();
        }


        //14.
        public static string DeleteProjectById(SoftUniContext context)
        {

            var empProjectDelete = context.EmployeesProjects
                .Where(p => p.Project.ProjectId == 2)
                .ToList();

            empProjectDelete.RemoveAll(p => p.ProjectId == 2);

            var projectDelete = context.Projects
                .FirstOrDefault(p => p.ProjectId == 2);

            context.Projects.Remove(projectDelete);

            var currProject = context.Projects
                .Take(10)
                .Select(p => new
                {
                    p.Name
                })
                .ToList();

            var sb = new StringBuilder();
            foreach (var cp in currProject) 
            {
                sb.AppendLine(cp.Name);
            }
            return sb.ToString().TrimEnd();
        }
    }
}
