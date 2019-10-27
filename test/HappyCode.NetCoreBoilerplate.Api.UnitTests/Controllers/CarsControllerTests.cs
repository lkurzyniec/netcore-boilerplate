using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HappyCode.NetCoreBoilerplate.Api.Controllers;
using HappyCode.NetCoreBoilerplate.Core.Dtos;
using HappyCode.NetCoreBoilerplate.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
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

        [Fact]
        public async Task Get_should_call_GetAllSortedByPlateAsync_onto_service()
        {
            //when
            await Controller.Get(default);

            //then
            _carServiceMock.Verify(x => x.GetAllSortedByPlateAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Get_should_return_Ok_with_expected_result()
        {
            //given
            _carServiceMock.Setup(x => x.GetAllSortedByPlateAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CarDto>
                {
                    new CarDto{},
                    new CarDto{},
                });

            //when
            var result = await Controller.Get(default) as OkObjectResult;

            //then
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(StatusCodes.Status200OK);
            result.Value.ShouldBeAssignableTo<IEnumerable<CarDto>>();
            var cars = result.Value as IEnumerable<CarDto>;
            cars.Count().ShouldBe(2);
        }
    }
}
