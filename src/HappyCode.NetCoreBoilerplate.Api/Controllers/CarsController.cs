using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HappyCode.NetCoreBoilerplate.Core;
using HappyCode.NetCoreBoilerplate.Core.Dtos;
using HappyCode.NetCoreBoilerplate.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace HappyCode.NetCoreBoilerplate.Api.Controllers
{
    [Route("api/cars")]
    public class CarsController : ApiControllerBase
    {
        private readonly ICarService _carService;

        public CarsController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CarDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            CancellationToken cancellationToken = default)
        {
            var result = await _carService.GetAllSortedByPlateAsync(cancellationToken);
            return Ok(result);
        }

        [FeatureGate(FeatureFlags.Santa)]
        [HttpGet("santa")]
        [ProducesResponseType(typeof(CarDto), StatusCodes.Status200OK)]
        public IActionResult GetSantaCar()
        {
            return Ok(new CarDto
            {
                Id = int.MaxValue,
                Model = "Magic Sleigh",
                Plate = "XMas 12",
            });
        }
    }
}
