using System;
using System.Diagnostics.CodeAnalysis;
using HappyCode.NetCoreBoilerplate.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace HappyCode.NetCoreBoilerplate.Api.IntegrationTests.Infrastructure.DataSeeders
{
    [ExcludeFromCodeCoverage]
    internal static class EmployeesDataSeeder
    {
        public static void Seed(DbContext dbContext, bool _)
        {
            Seed(dbContext);

            dbContext.SaveChanges();
        }

        public static Task SeedAsync(DbContext dbContext, bool _, CancellationToken token)
        {
            Seed(dbContext);

            return dbContext.SaveChangesAsync(token);
        }

        private static void Seed(DbContext dbContext)
        {
            var dept1 = new Department
            {
                Id =  Guid.NewGuid(),
                DeptName = "Test department",
            };
            dbContext.Set<Department>().Add(dept1);

            dbContext.Set<Employee>().Add(new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Thomas",
                LastName = "Anderson",
                BirthDate = new DateTime(1962, 03, 11),
                Gender = "M",
                Department = dept1,
            });

            dbContext.Set<Employee>().Add(new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Jonathan",
                LastName = "Fountain",
                BirthDate = new DateTime(1954, 07, 19),
                Gender = "M",
                Department = dept1,
            });

            dbContext.Set<Employee>().Add(new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Person",
                LastName = "ToDelete",
                BirthDate = new DateTime(2019, 10, 13),
                Gender = "M",
                Department = dept1,
            });
        }
    }
}
