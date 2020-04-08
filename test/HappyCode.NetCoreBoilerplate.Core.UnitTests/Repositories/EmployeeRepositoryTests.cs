using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
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
        private static readonly Fixture _fixture = new Fixture();

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
            var employees = _fixture.Build<Employee>()
                .Without(x => x.LeadingDepartments)
                .Without(x => x.Department)
                .CreateMany(20);

            _dbContextMock.Setup(x => x.Employees).Returns(employees.GetMockDbSetObject());

            //when
            var emp = await _repository.GetOldestAsync(default);

            //then
            var theOldest = employees.OrderBy(x => x.BirthDate).First();

            emp.Id.Should().Be(theOldest.EmpNo);
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
            var result = await _repository.DeleteByIdAsync(99, default);

            //then
            result.Should().Be(false);

            _dbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Theory, AutoData]
        public async Task DeleteByIdAsync_should_return_true_and_save_when_employee_found(int empId)
        {
            //given
            var employees = _fixture.Build<Employee>()
                .Without(x => x.LeadingDepartments)
                .Without(x => x.Department)
                .With(x => x.EmpNo, empId)
                .CreateMany(1);

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
