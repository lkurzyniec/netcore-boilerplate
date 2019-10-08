using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace HappyCode.NetCoreBoilerplate.Core.UnitTests.Infrastructure
{
    internal static class EnumerableExtensions
    {
        public static Mock<DbSet<TEntity>> GetMockDbSet<TEntity>(this IEnumerable<TEntity> source)
            where TEntity : class
        {
            var result = new Mock<DbSet<TEntity>>();

            var quarable = source.AsQueryable();
            result.As<IAsyncEnumerable<TEntity>>().Setup(x => x.GetEnumerator()).Returns(new TestAsyncEnumerator<TEntity>(quarable.GetEnumerator()));
            result.As<IQueryable<TEntity>>().Setup(dbSet => dbSet.Provider).Returns(new TestAsyncQueryProvider<TEntity>(quarable.Provider));
            result.As<IQueryable<TEntity>>().Setup(dbSet => dbSet.Expression).Returns(quarable.Expression);
            result.As<IQueryable<TEntity>>().Setup(dbSet => dbSet.ElementType).Returns(quarable.ElementType);
            result.As<IQueryable<TEntity>>().Setup(dbSet => dbSet.GetEnumerator()).Returns(quarable.GetEnumerator);

            return result;
        }
    }
}
