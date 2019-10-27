using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HappyCode.NetCoreBoilerplate.Core.Models;
using HappyCode.NetCoreBoilerplate.Core.Services;
using HappyCode.NetCoreBoilerplate.Core.UnitTests.Infrastructure;
using Moq;
using Shouldly;
using Xunit;

namespace HappyCode.NetCoreBoilerplate.Core.UnitTests.Repositories
{
    public class CarServiceTests
    {
        private readonly CarService _service;
        private readonly Mock<CarsContext> _dbContextMock;

        public CarServiceTests()
        {
            _dbContextMock = new Mock<CarsContext>();

            _service = new CarService(_dbContextMock.Object);
        }

        [Fact]
        public async Task GetOldestAsync_should_return_expected_employee()
        {
            //given
            var cars = new List<Car>
            {
                new Car{ Id = 1, Plate = "BB 44" },
                new Car{ Id = 2, Plate = "AA 44" },
                new Car{ Id = 3, Plate = "CC 44" },
            };
            _dbContextMock.Setup(x => x.Cars).Returns(cars.GetMockDbSet().Object);

            //when
            var result = await _service.GetAllSortedByPlateAsync(default);

            //then
            result.First().Id.ShouldBe(2);
        }
    }
}
