using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit3;
using FluentAssertions;
using HappyCode.NetCoreBoilerplate.Core.Dtos;
using HappyCode.NetCoreBoilerplate.Core.Models;
using HappyCode.NetCoreBoilerplate.Core.Repositories;
using HappyCode.NetCoreBoilerplate.Core.UnitTests.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace HappyCode.NetCoreBoilerplate.Core.UnitTests.Repositories
{
    public class EmployeeRepositoryTests
    {
        private static readonly Fixture _fixture = new Fixture();

        private readonly EmployeeRepository _repository;
        private readonly Mock<EmployeesContext> _dbContextMock;
        private readonly HybridCache _cache;

        public EmployeeRepositoryTests()
        {
            _dbContextMock = new Mock<EmployeesContext>(new DbContextOptionsBuilder<EmployeesContext>().Options);
            var distributedCacheMock = new Mock<IDistributedCache>();
            var memoryCacheMock = new Mock<IMemoryCache>();
            _repository = new EmployeeRepository(_dbContextMock.Object, _cache);
        }

        [Theory, AutoData]
        public async Task GetByIdAsync_should_return_expected_employee(Guid empId)
        {
            //given
            var employee = _fixture.Build<Employee>()
                .Without(x => x.LeadingDepartments)
                .Without(x => x.Department)
                .With(x => x.Id, empId)
                .Create();
            _dbContextMock.Setup(x => x.Employees).Returns(new[] { employee }.GetMockDbSetObject());

            //when
            var emp = await _repository.GetByIdAsync(empId, TestContext.Current.CancellationToken);

            //then
            emp.Id.Should().Be(empId);
        }

        [Fact]
        public async Task GetOldestAsync_should_return_expected_employee()
        {
            //given
            var employees = _fixture.Build<Employee>()
                .Without(x => x.LeadingDepartments)
                .Without(x => x.Department)
                .CreateMany(20);

            _dbContextMock.Setup(x => x.Employees).Returns(employees.GetMockDbSetObject());

            //when
            var emp = await _repository.GetOldestAsync(TestContext.Current.CancellationToken);

            //then
            var theOldest = employees.OrderBy(x => x.BirthDate).First();

            emp.Id.Should().Be(theOldest.Id);
            emp.LastName.Should()
                .NotBeNullOrEmpty()
                .And.Be(theOldest.LastName);
        }

        [Fact]
        public async Task DeleteByIdAsync_should_return_false_when_employee_not_found()
        {
            //given
            _dbContextMock.Setup(x => x.Employees).Returns(Enumerable.Empty<Employee>().GetMockDbSetObject);

            //when
            var result = await _repository.DeleteByIdAsync(Guid.NewGuid(), TestContext.Current.CancellationToken);

            //then
            result.Should().Be(false);

            _dbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Theory, AutoData]
        public async Task DeleteByIdAsync_should_return_true_and_save_when_employee_found(Guid empId)
        {
            //given
            var employees = _fixture.Build<Employee>()
                .Without(x => x.LeadingDepartments)
                .Without(x => x.Department)
                .With(x => x.Id, empId)
                .CreateMany(1);

            _dbContextMock.Setup(x => x.Employees).Returns(employees.GetMockDbSetObject());

            _dbContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            //when
            var result = await _repository.DeleteByIdAsync(empId, TestContext.Current.CancellationToken);

            //then
            result.Should().Be(true);

            _dbContextMock.Verify(x => x.Employees.Remove(It.Is<Employee>(y => y.Id == empId)), Times.Once);
            _dbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory, AutoData]
        public async Task InsertAsync_should_add_and_save(EmployeePostDto employeePostDto)
        {
            //given
            var employees = _fixture.Build<Employee>()
                .Without(x => x.LeadingDepartments)
                .Without(x => x.Department)
                .CreateMany(3);

            _dbContextMock.Setup(x => x.Employees).Returns(employees.GetMockDbSetObject());

            //when
            var result = await _repository.InsertAsync(employeePostDto, TestContext.Current.CancellationToken);

            //then
            result.Should().BeEquivalentTo(employeePostDto);

            _dbContextMock.Verify(x => x.Employees.AddAsync(It.Is<Employee>(y => y.LastName == employeePostDto.LastName), It.IsAny<CancellationToken>()), Times.Once);
            _dbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory, AutoData]
        public async Task UpdateAsync_should_return_null_when_employee_not_found(Guid empId, EmployeePutDto employeePutDto)
        {
            //given
            _dbContextMock.Setup(x => x.Employees).Returns(Enumerable.Empty<Employee>().GetMockDbSetObject);

            //when
            var result = await _repository.UpdateAsync(empId, employeePutDto, TestContext.Current.CancellationToken);

            //then
            result.Should().BeNull();

            _dbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Theory, AutoData]
        public async Task UpdateAsync_should_update_the_entity_and_save_when_employee_found(Guid empId, EmployeePutDto employeePutDto)
        {
            //given
            var employees = _fixture.Build<Employee>()
                .Without(x => x.LeadingDepartments)
                .Without(x => x.Department)
                .With(x => x.Id, empId)
                .CreateMany(1);

            _dbContextMock.Setup(x => x.Employees).Returns(employees.GetMockDbSetObject());

            //when
            var result = await _repository.UpdateAsync(empId, employeePutDto, TestContext.Current.CancellationToken);

            //then
            result.LastName.Should().Be(employeePutDto.LastName);

            _dbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
