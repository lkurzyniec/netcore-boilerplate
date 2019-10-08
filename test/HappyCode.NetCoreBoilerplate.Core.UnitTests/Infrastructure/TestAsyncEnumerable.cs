using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HappyCode.NetCoreBoilerplate.Core.UnitTests.Infrastructure
{
    /// <remarks>https://docs.microsoft.com/en-us/ef/ef6/fundamentals/testing/mocking</remarks>
    internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public TestAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        public TestAsyncEnumerable(Expression expression)
            : base(expression)
        { }

        public IAsyncEnumerator<T> GetEnumerator()
        {
            return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new TestAsyncQueryProvider<T>(this); }
        }
    }
}
