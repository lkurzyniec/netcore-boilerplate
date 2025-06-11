using System.Threading;
using System.Threading.Tasks;
using HappyCode.NetCoreBoilerplate.Core.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HappyCode.NetCoreBoilerplate.Core.Dtos;
using HappyCode.NetCoreBoilerplate.Core.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace HappyCode.NetCoreBoilerplate.Core.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<EmployeeDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<EmployeeDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<EmployeeDetailsDto> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken);
        Task<EmployeeDto> GetOldestAsync(CancellationToken cancellationToken);
        Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<EmployeeDto> InsertAsync(EmployeePostDto employeePostDto, CancellationToken cancellationToken);
        Task<EmployeeDto> UpdateAsync(Guid id, EmployeePutDto employeePutDto, CancellationToken cancellationToken);
    }

    internal class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(EmployeesContext dbContext) : base(dbContext)
        {

        }

        public async Task<List<EmployeeDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await GetAllAsync<EmployeeDto>(
                emp => emp.MapToDto(),
                cancellationToken);
        }

        public async Task<EmployeeDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await GetByIdAsync<EmployeeDto, Guid>(
                id,
                x => x.Id == id,
                emp => emp.MapToDto(),
                cancellationToken);
        }

        public async Task<EmployeeDetailsDto> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await GetByIdWithDetailsAsync<EmployeeDetailsDto, Guid>(
                id,
                x => x.Id == id,
                query => query.Include(x => x.Department),
                emp => new EmployeeDetailsDto
                {
                    Id = emp.Id,
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    BirthDate = emp.BirthDate,
                    Gender = emp.Gender,
                    Department = emp.Department != null ? new DepartmentDto
                    {
                        Id = emp.Department.Id,
                        Name = emp.Department.DeptName,
                    } : null
                },
                cancellationToken);
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

        public async Task<EmployeeDto> InsertAsync(EmployeePostDto employeePostDto, CancellationToken cancellationToken)
        {
            return await InsertAsync<EmployeeDto, EmployeePostDto>(
                employeePostDto,
                dto => new Employee
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    BirthDate = dto.BirthDate,
                    Gender = dto.Gender,
                    DeptId = dto.DeptId

                },
                emp => emp.MapToDto(),
                cancellationToken);
        }

        public async Task<EmployeeDto> UpdateAsync(Guid id, EmployeePutDto employeePutDto, CancellationToken cancellationToken)
        {
            return await UpdateAsync<EmployeeDto, EmployeePutDto, Guid>(
                id,
                employeePutDto,
                x => x.Id == id,
                (emp, dto) =>
                {
                    AssignIfNotNull(emp, value => emp.BirthDate = value, dto.BirthDate);
                    AssignIfNotNullOrEmpty(emp, value => emp.LastName = value, dto.LastName);
                    AssignIfNotNullOrEmpty(emp, value => emp.FirstName = value, dto.FirstName);
                    AssignIfNotNullOrEmpty(emp, value => emp.Gender = value, dto.Gender);
                    AssignIfNotNull(emp, value => emp.DeptId = value, dto.DeptId);
                },
                emp => emp.MapToDto(),
                cancellationToken);
        }

        public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await DeleteByIdAsync<Guid>(
                id,
                x => x.Id == id,
                cancellationToken);
        }
    }
}
