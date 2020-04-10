using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using HappyCode.NetCoreBoilerplate.Api.Controllers;
using HappyCode.NetCoreBoilerplate.Core.Dtos;
using HappyCode.NetCoreBoilerplate.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
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

        [Theory, AutoData]
        public async Task GetAll_should_return_expected_results(List<EmployeeDto> employees)
        {
            //given
            _employeeRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(employees);

            //when
            var result = await Controller.GetAll(default) as OkObjectResult;

            //then
            result.Should().NotBeNull();
            result.Value.Should().BeAssignableTo<IEnumerable<EmployeeDto>>()
                .And.BeEquivalentTo(employees);

            _employeeRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
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
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
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
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeAssignableTo<EmployeeDto>();
            var emp = result.Value as EmployeeDto;
            emp.Id.Should().Be(empId);
            emp.LastName.Should()
                .NotBeNullOrEmpty()
                .And.Be(lastName);
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
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
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
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }
    }
}
