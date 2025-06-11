using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

namespace HappyCode.NetCoreBoilerplate.Core.Repositories;

internal abstract class RepositoryBase<TEntity>
    where TEntity : class
{
    protected EmployeesContext DbContext { get; }
    private readonly HybridCache _cache;
    private readonly string _entityTypeName;

    protected RepositoryBase(EmployeesContext dbContext, HybridCache cache)
    {
        DbContext = dbContext;
        _cache = cache;
        _entityTypeName = typeof(TEntity).Name;
    }

    // Helper methods for cache key generation
    protected virtual string GetAllCacheKey() => $"{_entityTypeName}:All";
    protected virtual string GetByIdCacheKey<TKey>(TKey id) => $"{_entityTypeName}:{id}";
    protected virtual string GetDetailsCacheKey<TKey>(TKey id) => $"{_entityTypeName}:Details:{id}";

    // Invalidate all cache entries for this entity type
    protected virtual async Task InvalidateCacheAsync()
    {
        await _cache.RemoveAsync(GetAllCacheKey());
    }

    // Invalidate cache entry for a specific entity
    protected virtual async Task InvalidateEntityCacheAsync<TKey>(TKey id)
    {
        await _cache.RemoveAsync(GetByIdCacheKey(id));
        await _cache.RemoveAsync(GetDetailsCacheKey(id));
        await InvalidateCacheAsync(); // Also invalidate the collection cache
    }

    protected async Task<List<TDto>> GetAllAsync<TDto>(
      Func<TEntity, TDto> mapper,
      CancellationToken cancellationToken)
    {
        var cacheKey = GetAllCacheKey();

        // Use GetOrCreateAsync to get from cache or create if not found
        return await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {

            await Task.Delay(8000);
            var entities = await DbContext.Set<TEntity>()
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return entities.Select(mapper).ToList();
        });
    }

    // Generic method for GetByIdAsync with GetOrCreateAsync
    protected async Task<TDto> GetByIdAsync<TDto, TKey>(
        TKey id,
        Expression<Func<TEntity, bool>> idPredicate,
        Func<TEntity, TDto> mapper,
        CancellationToken cancellationToken)
    {
        var cacheKey = GetByIdCacheKey(id);

        // Use GetOrCreateAsync to get from cache or create if not found
        return await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            var entity = await DbContext.Set<TEntity>()
                .AsNoTracking()
                .SingleOrDefaultAsync(idPredicate, cancellationToken);

            return entity == null ? default : mapper(entity);
        });
    }

    // Generic method for GetByIdWithDetailsAsync with GetOrCreateAsync
    protected async Task<TDto> GetByIdWithDetailsAsync<TDto, TKey>(
        TKey id,
        Expression<Func<TEntity, bool>> idPredicate,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includeFunc,
        Func<TEntity, TDto> mapper,
        CancellationToken cancellationToken)
    {
        var cacheKey = GetDetailsCacheKey(id);

        // Use GetOrCreateAsync to get from cache or create if not found
        return await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            var query = DbContext.Set<TEntity>().AsQueryable();
            query = includeFunc(query);

            var entity = await query.SingleOrDefaultAsync(idPredicate, cancellationToken);

            return entity == null ? default : mapper(entity);
        });
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

        // Invalidate cache after insert
        await InvalidateCacheAsync();

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

        // Invalidate cache after update
        await InvalidateEntityCacheAsync(id);

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
        var result = await DbContext.SaveChangesAsync(cancellationToken) > 0;

        // Invalidate cache after delete
        if (result)
        {
            await InvalidateEntityCacheAsync(id);
        }

        return result;
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
