using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using HappyCode.NetCoreBoilerplate.Core.Dtos;
using HappyCode.NetCoreBoilerplate.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.IdentityModel.Tokens;

namespace HappyCode.NetCoreBoilerplate.Core.Repositories
{
    public interface IDepartmentRepository
    {
        Task<List<DepartmentDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<DepartmentDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<DepartmentDetailsDto> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<DepartmentDto> InsertAsync(DepartmentPostDto departmentPostDto, CancellationToken cancellationToken);
        Task<DepartmentDto> UpdateAsync(Guid id, DepartmentPutDto departmentPutDto, CancellationToken cancellationToken);
    }

    internal class DepartmentRepository : RepositoryBase<Department>, IDepartmentRepository
    {

        private readonly HybridCache _distributedCache;

        public DepartmentRepository(EmployeesContext dbContext, HybridCache cache) : base(dbContext, cache)
        {
            _distributedCache = cache;
        }

        public async Task<List<DepartmentDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await GetAllAsync<DepartmentDto>(
                dept => new DepartmentDto
                {
                    Id = dept.Id,
                    Name = dept.DeptName
                },
                cancellationToken);
        }

        public async Task<DepartmentDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await GetByIdAsync<DepartmentDto, Guid>(
                id,
                x => x.Id == id,
                dept => new DepartmentDto
                {
                    Id = dept.Id,
                    Name = dept.DeptName
                },
                cancellationToken);
        }

        public async Task<DepartmentDetailsDto> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await GetByIdWithDetailsAsync<DepartmentDetailsDto, Guid>(
                id,
                x => x.Id == id,
                query => query.Include(d => d.Employees)
                             .Include(d => d.Manger),
                dept => new DepartmentDetailsDto
                {
                    Id = dept.Id,
                    Name = dept.DeptName,
                    Manager = dept.Manger != null ? new EmployeeDto
                    {
                        Id = dept.Manger.Id,
                        FirstName = dept.Manger.FirstName,
                        LastName = dept.Manger.LastName,
                        BirthDate = dept.Manger.BirthDate,
                        Gender = dept.Manger.Gender
                    } : null,
                    Employees = dept.Employees.Select(e => new EmployeeDto
                    {
                        Id = e.Id,
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        BirthDate = e.BirthDate,
                        Gender = e.Gender
                    }).ToList()
                },
                cancellationToken);
        }

        public async Task<DepartmentDto> InsertAsync(DepartmentPostDto departmentPostDto, CancellationToken cancellationToken)
        {
            return await InsertAsync<DepartmentDto, DepartmentPostDto>(
                departmentPostDto,
                dto => new Department
                {
                    DeptName = dto.Name,
                    MangerId = dto.ManagerId
                },
                dept => new DepartmentDto
                {
                    Id = dept.Id,
                    Name = dept.DeptName
                },
                cancellationToken);
        }

        public async Task<DepartmentDto> UpdateAsync(Guid id, DepartmentPutDto departmentPutDto, CancellationToken cancellationToken)
        {
            return await UpdateAsync<DepartmentDto, DepartmentPutDto, Guid>(
                id,
                departmentPutDto,
                x => x.Id == id,
                (dept, dto) =>
                {
                    AssignIfNotNullOrEmpty(dept, value => dept.DeptName = value, dto.Name);
                    if (dto.ManagerId.HasValue)
                    {
                        dept.MangerId = dto.ManagerId.Value;
                    }
                },
                dept => new DepartmentDto
                {
                    Id = dept.Id,
                    Name = dept.DeptName
                },
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
