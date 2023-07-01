using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HappyCode.NetCoreBoilerplate.Core.Dtos;
using Microsoft.EntityFrameworkCore;

namespace HappyCode.NetCoreBoilerplate.Core.Services
{
    public interface ICarService
    {
        Task<IEnumerable<CarDto>> GetAllSortedByPlateAsync(CancellationToken cancellationToken);
    }

    internal class CarService : ICarService
    {
        private readonly CarsContext _dbContext;

        public CarService(CarsContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<CarDto>> GetAllSortedByPlateAsync(CancellationToken cancellationToken)
        {
            var cars = await _dbContext.Cars
                .AsNoTracking()
                .OrderBy(x => x.Plate)
                .ToListAsync(cancellationToken);

            return cars.Select(x => new CarDto
            {
                Id = x.Id,
                Plate = x.Plate,
                Model = x.Model,
            });
        }
    }
}
