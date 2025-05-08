using System.Diagnostics.CodeAnalysis;
using HappyCode.NetCoreBoilerplate.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace HappyCode.NetCoreBoilerplate.Api.IntegrationTests.Infrastructure.DataSeeders
{
    [ExcludeFromCodeCoverage]
    internal static class CarsDataSeeder
    {
        public static void Seed(DbContext dbContext, bool _)
        {
            Seed(dbContext);

            dbContext.SaveChanges();
        }

        public static async Task SeedAsync(DbContext dbContext, bool _, CancellationToken token)
        {
            Seed(dbContext);

            await dbContext.SaveChangesAsync(token);
        }


        private static void Seed(DbContext dbContext)
        {
            var cars = new[]
                        {
                new Car
                {
                    Plate = "DW 12345",
                    Model = "Toyota Avensis",
                },
                new Car
                {
                    Plate = "SB 98765",
                    Model = "Mercedes-Benz",
                },
            };
            dbContext.Set<Car>().AddRange(cars);
        }
    }
}
