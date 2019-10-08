using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HappyCode.NetCoreBoilerplate.Core;
using HappyCode.NetCoreBoilerplate.Core.Models;
using HappyCode.NetCoreBoilerplate.Core.Repositories;
using HappyCode.NetCoreBoilerplate.Core.UnitTests.Infrastructure;
using Moq;
using Shouldly;
using Xunit;

namespace HappyCode.NetCoreBoilerplate.Core.UnitTests.Repositories
{
    public class EmployeeRepositoryTests
    {
        private readonly EmployeeRepository _repository;
        private readonly Mock<EmployeesContext> _dbContextMock;

        public EmployeeRepositoryTests()
        {
            _dbContextMock = new Mock<EmployeesContext>();

            _repository = new EmployeeRepository(_dbContextMock.Object);
        }

        [Fact]
        public async Task Db_should_return_expected_employee()
        {
            //given
            var users = new List<Employee>
            {
                new Employee { EmpNo = 22, LastName = "Richard", BirthDate = new DateTime(1983, 07, 21) },
                new Employee { EmpNo = 45, LastName = "Hudson", BirthDate = new DateTime(1962, 09, 30) },
                new Employee { EmpNo = 54, LastName = "Bias", BirthDate = new DateTime(1976, 11, 11) },
            };

            _dbContextMock.Setup(x => x.Employees).Returns(users.GetMockDbSet().Object);

            //when
            var emp = await _repository.GetOldestAsync(default);

            //then
            emp.Id.ShouldBe(45);
            emp.LastName.ShouldBe("Hudson");
        }
    }
}
