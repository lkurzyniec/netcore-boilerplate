using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit3;
using FluentAssertions;
using HappyCode.NetCoreBoilerplate.Core.Models;
using HappyCode.NetCoreBoilerplate.Core.Services;
using HappyCode.NetCoreBoilerplate.Core.UnitTests.Extensions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace HappyCode.NetCoreBoilerplate.Core.UnitTests.Services
{
    public class CarServiceTests
    {
        private static readonly Fixture _fixture = new Fixture();

        private readonly CarService _service;
        private readonly Mock<CarsContext> _dbContextMock;

        public CarServiceTests()
        {
            _dbContextMock = new Mock<CarsContext>(new DbContextOptionsBuilder<CarsContext>().Options);

            _service = new CarService(_dbContextMock.Object);
        }

        [Theory, AutoData]
        public async Task GetAllSortedByPlateAsync_should_return_expected_result(int rand1, int rand2, int expectedId)
        {
            //given
            _fixture.Customize<Car>(c => c.Without(x => x.Owner));

            var cars = new List<Car>();
            _fixture.AddManyTo(cars, rand1);
            cars.Add(new Car { Id = expectedId, Plate = "0" });
            _fixture.AddManyTo(cars, rand2);

            _dbContextMock.Setup(x => x.Cars).Returns(cars.GetMockDbSetObject());

            //when
            var result = await _service.GetAllSortedByPlateAsync(TestContext.Current.CancellationToken);

            //then
            result.First().Id.Should().Be(expectedId);
        }
    }
}
