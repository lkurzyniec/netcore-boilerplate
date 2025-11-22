using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit3;
using AwesomeAssertions;
using HappyCode.NetCoreBoilerplate.Api.Controllers;
using HappyCode.NetCoreBoilerplate.Core.Dtos;
using HappyCode.NetCoreBoilerplate.Core.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace HappyCode.NetCoreBoilerplate.Api.UnitTests.Controllers
{
    public class CarsControllerTests : ControllerTestsBase<CarsController>
    {
        private readonly Mock<ICarService> _carServiceMock;

        public CarsControllerTests()
        {
            _carServiceMock = Mocker.GetMock<ICarService>();
        }

        [Theory, AutoData]
        public async Task GetAll_should_return_Ok_with_expected_result(IEnumerable<CarDto> cars)
        {
            //given
            _carServiceMock.Setup(x => x.GetAllSortedByPlateAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(cars);

            //when
            var result = await Controller.GetAllAsync(TestContext.Current.CancellationToken);

            //then
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeAssignableTo<IEnumerable<CarDto>>()
                .And.HaveCount(cars.Count());

            _carServiceMock.VerifyAll();
        }
    }
}
