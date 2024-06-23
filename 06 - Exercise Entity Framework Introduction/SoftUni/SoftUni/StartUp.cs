﻿using SoftUni.Data;
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
            Console.WriteLine(GetEmployeesFullInformation(context));
        }

        //01.
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


    }
}
