using HappyCode.NetCoreBoilerplate.Core;
using HappyCode.NetCoreBoilerplate.Core.Dtos;
using HappyCode.NetCoreBoilerplate.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace HappyCode.NetCoreBoilerplate.Api.Controllers
{
    [FeatureGate(FeatureFlags.DockerCompose)]
    [Route("api/cars")]
    public class CarsController(ICarService carService) : ApiControllerBase
    {
        private readonly ICarService _carService = carService;

        [HttpGet]
        public async Task<Ok<IEnumerable<CarDto>>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var result = await _carService.GetAllSortedByPlateAsync(cancellationToken);
            return TypedResults.Ok(result);
        }

        [FeatureGate(FeatureFlags.Santa)]
        [HttpGet("santa")]
        public Ok<CarDto> GetSantaCar()
        {
            return TypedResults.Ok(new CarDto
            {
                Id = int.MaxValue,
                Model = "Magic Sleigh",
                Plate = "XMas 12",
            });
        }
    }
}
