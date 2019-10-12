using System;
using HappyCode.NetCoreBoilerplate.Core;
using HappyCode.NetCoreBoilerplate.Core.Models;

namespace HappyCode.NetCoreBoilerplate.Api.IntegrationTests.Infrastructure
{
    public class EmployeeContextDataFeeder
    {
        public static void Feed(EmployeesContext dbContext)
        {
            var dept1 = new Department
            {
                DeptNo = "D1",
                DeptName = "Test department",
            };
            dbContext.Departments.Add(dept1);

            var emp1 = new Employee
            {
                EmpNo = 1,
                FirstName = "Thomas",
                LastName = "Anderson",
                BirthDate = new DateTime(1962, 03, 11),
                Gender = "M",
                Department = dept1,
            };
            dbContext.Employees.Add(emp1);

            var emp2 = new Employee
            {
                EmpNo = 99,
                FirstName = "Person",
                LastName = "ToDelete",
                BirthDate = new DateTime(2019, 10, 13),
                Gender = "M",
                Department = dept1,
            };
            dbContext.Employees.Add(emp2);

            dbContext.SaveChanges();
        }
    }
}
