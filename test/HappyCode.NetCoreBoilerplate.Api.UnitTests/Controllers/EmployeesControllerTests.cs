using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit3;
using AwesomeAssertions;
using HappyCode.NetCoreBoilerplate.Api.Controllers;
using HappyCode.NetCoreBoilerplate.Core;
using HappyCode.NetCoreBoilerplate.Core.Dtos;
using HappyCode.NetCoreBoilerplate.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
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
            var result = await Controller.GetAllAsync(TestContext.Current.CancellationToken) as OkObjectResult;

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
            await Controller.GetAsync(11, TestContext.Current.CancellationToken);

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
            var result = await Controller.GetAsync(1, TestContext.Current.CancellationToken) as StatusCodeResult;

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
            var result = await Controller.GetAsync(1, TestContext.Current.CancellationToken) as OkObjectResult;

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
            await Controller.DeleteAsync(11, TestContext.Current.CancellationToken);

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
            var result = await Controller.DeleteAsync(1, TestContext.Current.CancellationToken) as StatusCodeResult;

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
            var result = await Controller.DeleteAsync(1, TestContext.Current.CancellationToken) as StatusCodeResult;

            //then
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Theory, AutoData]
        public async Task Put_should_return_NotFound_when_repository_return_null(int empId, EmployeePutDto employeePutDto)
        {
            //given
            _employeeRepositoryMock.Setup(x => x.UpdateAsync(empId, employeePutDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null)
                .Verifiable();

            //when
            var result = await Controller.PutAsync(empId, employeePutDto, TestContext.Current.CancellationToken) as StatusCodeResult;

            //then
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            _employeeRepositoryMock.Verify();
        }

        [Theory, AutoData]
        public async Task Put_should_return_Ok_with_result_when_update_finished_with_success(int empId, EmployeePutDto employeePutDto, EmployeeDto employee)
        {
            //given
            _employeeRepositoryMock.Setup(x => x.UpdateAsync(empId, employeePutDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(employee)
                .Verifiable();

            //when
            var result = await Controller.PutAsync(empId, employeePutDto, TestContext.Current.CancellationToken) as OkObjectResult;

            //then
            result.Value.Should().BeAssignableTo<EmployeeDto>()
                .And.BeEquivalentTo(employee);

            _employeeRepositoryMock.Verify();
        }

        [Theory, AutoData]
        public async Task Post_should_return_Created_with_result_and_header_when_insert_finished_with_success(EmployeePostDto employeePostDto, EmployeeDto employee)
        {
            //given
            _employeeRepositoryMock.Setup(x => x.InsertAsync(employeePostDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(employee)
                .Verifiable();

            //when
            var result = await Controller.PostAsync(employeePostDto, TestContext.Current.CancellationToken) as ObjectResult;

            //then
            result.StatusCode.Should().Be(StatusCodes.Status201Created);
            result.Value.Should().BeAssignableTo<EmployeeDto>()
                .And.BeEquivalentTo(employee);

            Controller.HttpContext.Response.Headers.TryGetValue("x-date-created", out _).Should().BeTrue();

            _employeeRepositoryMock.Verify();
        }

        [Fact]
        public async Task GetOldest_should_return_Santa_when_feature_enabled()
        {
            //given
            var featureManagerMock = Mocker.GetMock<IFeatureManager>();
            featureManagerMock.Setup(x => x.IsEnabledAsync(FeatureFlags.Santa.ToString()))
                .ReturnsAsync(true)
                .Verifiable();

            //when
            var result = await Controller.GetOldestAsync(TestContext.Current.CancellationToken) as ObjectResult;

            //then
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeAssignableTo<EmployeeDto>()
                .Which.FirstName.Should().Be("Santa");
        }
    }
}
