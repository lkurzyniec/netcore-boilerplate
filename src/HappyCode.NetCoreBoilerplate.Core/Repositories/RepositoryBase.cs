using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HappyCode.NetCoreBoilerplate.Core.Repositories;

internal abstract class RepositoryBase<TEntity>
    where TEntity : class
{
    protected EmployeesContext DbContext { get; }

    protected RepositoryBase(EmployeesContext dbContext)
    {
        DbContext = dbContext;
    }

    protected async Task<List<TDto>> GetAllAsync<TDto>(
      Func<TEntity, TDto> mapper,
      CancellationToken cancellationToken)
    {
        var entities = await DbContext.Set<TEntity>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return entities.Select(mapper).ToList();
    }

    // Generic method for GetByIdAsync
    protected async Task<TDto> GetByIdAsync<TDto, TKey>(
        TKey id,
        Expression<Func<TEntity, bool>> idPredicate,
        Func<TEntity, TDto> mapper,
        CancellationToken cancellationToken)
    {
        var entity = await DbContext.Set<TEntity>()
            .AsNoTracking()
            .SingleOrDefaultAsync(idPredicate, cancellationToken);

        return entity == null ? default : mapper(entity);
    }

    // Generic method for GetByIdWithDetailsAsync
    protected async Task<TDto> GetByIdWithDetailsAsync<TDto, TKey>(
        TKey id,
        Expression<Func<TEntity, bool>> idPredicate,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includeFunc,
        Func<TEntity, TDto> mapper,
        CancellationToken cancellationToken)
    {
        var query = DbContext.Set<TEntity>().AsQueryable();
        query = includeFunc(query);

        var entity = await query.SingleOrDefaultAsync(idPredicate, cancellationToken);

        return entity == null ? default : mapper(entity);
    }

    protected async Task<TDto> InsertAsync<TDto, TCreateDto>(
        TCreateDto createDto,
        Func<TCreateDto, TEntity> createMapper,
        Func<TEntity, TDto> resultMapper,
        CancellationToken cancellationToken)
    {
        var entity = createMapper(createDto);

        await DbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);

        return resultMapper(entity);
    }

    // Generic Update method
    protected async Task<TDto> UpdateAsync<TDto, TUpdateDto, TKey>(
        TKey id,
        TUpdateDto updateDto,
        Expression<Func<TEntity, bool>> idPredicate,
        Action<TEntity, TUpdateDto> updateAction,
        Func<TEntity, TDto> resultMapper,
        CancellationToken cancellationToken)
    {
        var entity = await DbContext.Set<TEntity>()
            .SingleOrDefaultAsync(idPredicate, cancellationToken);

        if (entity == null)
            return default;

        updateAction(entity, updateDto);

        await DbContext.SaveChangesAsync(cancellationToken);
        return resultMapper(entity);
    }

    // Generic Delete method
    protected async Task<bool> DeleteByIdAsync<TKey>(
        TKey id,
        Expression<Func<TEntity, bool>> idPredicate,
        CancellationToken cancellationToken)
    {
        var entity = await DbContext.Set<TEntity>()
            .SingleOrDefaultAsync(idPredicate, cancellationToken);

        if (entity == null)
            return false;

        DbContext.Set<TEntity>().Remove(entity);
        return await DbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    protected void AssignIfNotNull<TValue>(TEntity entity, Action<TValue> setter, TValue? source) where TValue : struct
    {
        if (source.HasValue)
        {
            setter(source.Value);
        }
    }

    protected void AssignIfNotNullOrEmpty(TEntity entity, Action<string> setter, string? source)
    {
        if (!string.IsNullOrEmpty(source))
        {
            setter(source);
        }
    }
}
