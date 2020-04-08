using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HappyCode.NetCoreBoilerplate.Core.Models;
using HappyCode.NetCoreBoilerplate.Core.Repositories;
using HappyCode.NetCoreBoilerplate.Core.UnitTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace HappyCode.NetCoreBoilerplate.Core.UnitTests.Repositories
{
    public class EmployeeRepositoryTests
    {
        private readonly EmployeeRepository _repository;
        private readonly Mock<EmployeesContext> _dbContextMock;

        public EmployeeRepositoryTests()
        {
            _dbContextMock = new Mock<EmployeesContext>(new DbContextOptionsBuilder<EmployeesContext>().Options);

            _repository = new EmployeeRepository(_dbContextMock.Object);
        }

        [Fact]
        public async Task GetOldestAsync_should_return_expected_employee()
        {
            //given
            var employees = new List<Employee>
            {
                new Employee { EmpNo = 22, LastName = "Richard", BirthDate = new DateTime(1983, 07, 21) },
                new Employee { EmpNo = 45, LastName = "Hudson", BirthDate = new DateTime(1962, 09, 30) },
                new Employee { EmpNo = 54, LastName = "Bias", BirthDate = new DateTime(1976, 11, 11) },
            };
            _dbContextMock.Setup(x => x.Employees).Returns(employees.GetMockDbSetObject());

            //when
            var emp = await _repository.GetOldestAsync(default);

            //then
            emp.Id.Should().Be(45);
            emp.LastName.Should()
                .NotBeNullOrEmpty()
                .And.Be("Hudson");
        }

        [Fact]
        public async Task DeleteByIdAsync_should_return_false_when_employee_not_found()
        {
            //given
            _dbContextMock.Setup(x => x.Employees).Returns(Enumerable.Empty<Employee>().GetMockDbSetObject);

            //when
            var result = await _repository.DeleteByIdAsync(99, default);

            //then
            result.Should().Be(false);

            _dbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task DeleteByIdAsync_should_return_true_and_save_when_employee_found()
        {
            //given
            const int empId = 22;

            var employees = new List<Employee>
            {
                new Employee { EmpNo = empId },
            };
            _dbContextMock.Setup(x => x.Employees).Returns(employees.GetMockDbSetObject());

            _dbContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            //when
            var result = await _repository.DeleteByIdAsync(empId, default);

            //then
            result.Should().Be(true);

            _dbContextMock.Verify(x => x.Employees.Remove(It.Is<Employee>(y => y.EmpNo == empId)), Times.Once);
            _dbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
