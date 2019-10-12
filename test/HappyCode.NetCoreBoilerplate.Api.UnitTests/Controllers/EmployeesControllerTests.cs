using System.Threading;
using System.Threading.Tasks;
using HappyCode.NetCoreBoilerplate.Api.Controllers;
using HappyCode.NetCoreBoilerplate.Core.Dtos;
using HappyCode.NetCoreBoilerplate.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Xunit;

namespace HappyCode.NetCoreBoilerplate.Api.UnitTests.Controllers
{
    public class EmployeesControllerTests : ControllerTestsBase<EmployeesController>
    {
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;

        public EmployeesControllerTests()
        {
            _employeeRepositoryMock = Mocker.GetMock<IEmployeeRepository>();
        }

        [Fact]
        public async Task Get_should_call_GetByIdAsync_onto_repository()
        {
            //given
            const int empId = 11;

            //when
            await Controller.Get(11, default);

            //then
            _employeeRepositoryMock.Verify(x => x.GetByIdAsync(empId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Get_should_return_NotFound_when_repository_return_null()
        {
            //given
            _employeeRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            //when
            var result = await Controller.Get(1, default) as StatusCodeResult;

            //then
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task Get_should_return_Ok_with_expected_result_when_repository_return_object()
        {
            //given
            const int empId = 22;
            const string lastName = "Smith";

            _employeeRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new EmployeeDto
                {
                    Id = empId,
                    LastName = lastName,
                });

            //when
            var result = await Controller.Get(1, default) as OkObjectResult;

            //then
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(StatusCodes.Status200OK);
            result.Value.ShouldBeAssignableTo<EmployeeDto>();
            var emp = result.Value as EmployeeDto;
            emp.Id.ShouldBe(empId);
            emp.LastName.ShouldBe(lastName);
        }

        [Fact]
        public async Task Delete_should_call_DeleteByIdAsync_onto_repository()
        {
            //given
            const int empId = 11;

            //when
            await Controller.Delete(11, default);

            //then
            _employeeRepositoryMock.Verify(x => x.DeleteByIdAsync(empId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Delete_should_return_NotFound_when_repository_return_false()
        {
            //given
            _employeeRepositoryMock.Setup(x => x.DeleteByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            //when
            var result = await Controller.Delete(1, default) as StatusCodeResult;

            //then
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task Delete_should_return_NoContent_when_repository_return_true()
        {
            //given
            _employeeRepositoryMock.Setup(x => x.DeleteByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            //when
            var result = await Controller.Delete(1, default) as StatusCodeResult;

            //then
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(StatusCodes.Status204NoContent);
        }
    }
}
