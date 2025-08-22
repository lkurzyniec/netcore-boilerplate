using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;

namespace HappyCode.NetCoreBoilerplate.Core.UnitTests.Extensions
{
    internal static class EnumerableExtensions
    {
        public static DbSet<TEntity> GetMockDbSetObject<TEntity>(this IEnumerable<TEntity> source)
            where TEntity : class
        {
            return source.ToList().BuildMockDbSet().Object;
        }
    }
}
