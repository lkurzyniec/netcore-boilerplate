using HappyCode.NetCoreBoilerplate.Core;
using HappyCode.NetCoreBoilerplate.Core.Dtos;
using HappyCode.NetCoreBoilerplate.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace HappyCode.NetCoreBoilerplate.Api.Controllers
{
    [FeatureGate(FeatureFlags.DockerCompose)]
    [Route("api/employees")]
    public class EmployeesController : ApiControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IFeatureManager _featureManager;

        public EmployeesController(IEmployeeRepository employeeRepository, IFeatureManager featureManager)
        {
            _employeeRepository = employeeRepository;
            _featureManager = featureManager;
        }

        [HttpGet]
        public async Task<Ok<List<EmployeeDto>>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var result = await _employeeRepository.GetAllAsync(cancellationToken);
            return TypedResults.Ok(result);
        }

        [HttpGet("{id}", Name = nameof(GetAsync))]
        public async Task<Results<Ok<EmployeeDto>, NotFound>> GetAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var result = await _employeeRepository.GetByIdAsync(id, cancellationToken);
            if (result == null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(result);
        }

        [HttpGet("{id}/details")]
        public async Task<Results<Ok<EmployeeDetailsDto>, NotFound>> GetWithDetailsAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var result = await _employeeRepository.GetByIdWithDetailsAsync(id, cancellationToken);
            if (result == null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(result);
        }

        [HttpGet("oldest")]
        public async Task<Results<Ok<EmployeeDto>, NotFound>> GetOldestAsync(
            CancellationToken cancellationToken = default)
        {
            if (await _featureManager.IsEnabledAsync(FeatureFlags.Santa.ToString()))
            {
                return TypedResults.Ok(new EmployeeDto
                {
                    Id = int.MaxValue,
                    FirstName = "Santa",
                    LastName = "Claus",
                    BirthDate = new DateTime(270, 3, 15),
                    Gender = "M",
                });
            }

            var result = await _employeeRepository.GetOldestAsync(cancellationToken);
            if (result == null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<Results<Ok<EmployeeDto>, NotFound, BadRequest<HttpValidationProblemDetails>>> PutAsync(
            [FromRoute] int id,
            [FromBody] EmployeePutDto employeePutDto,
            CancellationToken cancellationToken = default)
        {
            var result = await _employeeRepository.UpdateAsync(id, employeePutDto, cancellationToken);
            if (result is null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(result);
        }

        [HttpPost]
        public async Task<Results<CreatedAtRoute<EmployeeDto>, BadRequest<HttpValidationProblemDetails>>> PostAsync(
            [FromBody] EmployeePostDto employeePostDto,
            CancellationToken cancellationToken = default)
        {
            var result = await _employeeRepository.InsertAsync(employeePostDto, cancellationToken);
            Response.Headers.Append("x-date-created", DateTime.UtcNow.ToString("s"));
            return TypedResults.CreatedAtRoute(result, nameof(GetAsync), new RouteValueDictionary(new { id = result.Id }));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<NoContent, NotFound>> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var result = await _employeeRepository.DeleteByIdAsync(id, cancellationToken);
            if (result)
            {
                return TypedResults.NoContent();
            }
            return TypedResults.NotFound();
        }
    }
}
