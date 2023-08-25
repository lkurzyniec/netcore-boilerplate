using HappyCode.NetCoreBoilerplate.Core;
using HappyCode.NetCoreBoilerplate.Core.Models;

namespace HappyCode.NetCoreBoilerplate.Api.IntegrationTests.Infrastructure.DataFeeders
{
    internal static class CarsContextDataFeeder
    {
        public static void Feed(CarsContext dbContext)
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
            dbContext.Cars.AddRange(cars);
            dbContext.SaveChanges();
        }
    }
}
