using HappyCode.NetCoreBoilerplate.Core;
using HappyCode.NetCoreBoilerplate.Core.Dtos;
using HappyCode.NetCoreBoilerplate.Core.Repositories;
using Microsoft.AspNetCore.Http;
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
        [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var result = await _employeeRepository.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var result = await _employeeRepository.GetByIdAsync(id, cancellationToken);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("{id}/details")]
        [ProducesResponseType(typeof(EmployeeDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWithDetailsAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var result = await _employeeRepository.GetByIdWithDetailsAsync(id, cancellationToken);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("oldest")]
        [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOldestAsync(
            CancellationToken cancellationToken = default)
        {
            if (await _featureManager.IsEnabledAsync(FeatureFlags.Santa.ToString()))
            {
                return Ok(new EmployeeDto
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
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromBody] EmployeePutDto employeePutDto,
            CancellationToken cancellationToken = default)
        {
            var result = await _employeeRepository.UpdateAsync(id, employeePutDto, cancellationToken);
            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> PostAsync(
            [FromBody] EmployeePostDto employeePostDto,
            CancellationToken cancellationToken = default)
        {
            var result = await _employeeRepository.InsertAsync(employeePostDto, cancellationToken);
            Response.Headers.Append("x-date-created", DateTime.UtcNow.ToString("s"));
            return CreatedAtAction("Get", new { id = result.Id }, result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var result = await _employeeRepository.DeleteByIdAsync(id, cancellationToken);
            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
