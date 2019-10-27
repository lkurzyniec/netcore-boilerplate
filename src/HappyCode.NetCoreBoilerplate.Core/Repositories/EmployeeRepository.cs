using System.Threading;
using System.Threading.Tasks;
using HappyCode.NetCoreBoilerplate.Core.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HappyCode.NetCoreBoilerplate.Core.Dtos;
using HappyCode.NetCoreBoilerplate.Core.Extensions;
using System.Collections.Generic;

namespace HappyCode.NetCoreBoilerplate.Core.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<EmployeeDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<EmployeeDto> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<EmployeeDetailsDto> GetByIdWithDetailsAsync(object id, CancellationToken cancellationToken);
        Task<EmployeeDto> GetOldestAsync(CancellationToken cancellationToken);
        Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken);
    }

    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(EmployeesContext dbContext) : base(dbContext)
        {

        }

        public async Task<List<EmployeeDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var employees = await DbContext.Employees
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return employees.Select(EmployeeExtensions.MapToDto).ToList();
        }

        public async Task<EmployeeDto> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var emp = await DbContext.Employees
                .SingleOrDefaultAsync(x => x.EmpNo == id);
            if (emp == null)
            {
                return null;
            }

            return emp.MapToDto();
        }

        public async Task<EmployeeDetailsDto> GetByIdWithDetailsAsync(object id, CancellationToken cancellationToken)
        {
            var emp = await DbContext.Employees
                .Include(x => x.Department)
                .SingleOrDefaultAsync(x => x.EmpNo == (int)id, cancellationToken);
            if (emp == null)
            {
                return null;
            }

            return new EmployeeDetailsDto
            {
                Id = emp.EmpNo,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                BirthDate = emp.BirthDate,
                Gender = emp.Gender,
                Department = new DepartmentDto
                {
                    Id = emp.Department.DeptNo,
                    Name = emp.Department.DeptName,
                }
            };
        }

        public async Task<EmployeeDto> GetOldestAsync(CancellationToken cancellationToken)
        {
            var emp = await DbContext.Employees
                .OrderBy(x => x.BirthDate)
                .FirstOrDefaultAsync(cancellationToken);
            if (emp == null)
            {
                return null;
            }

            return emp.MapToDto();
        }

        public async Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken)
        {
            var emp = await DbContext.Employees
                .SingleOrDefaultAsync(x => x.EmpNo == id);
            if (emp == null)
            {
                return false;
            }

            DbContext.Employees.Remove(emp);
            return await DbContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
