using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HappyCode.NetCoreBoilerplate.Core.UnitTests.Infrastructure
{
    /// <remarks>https://docs.microsoft.com/en-us/ef/ef6/fundamentals/testing/mocking</remarks>
    internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public T Current => _inner.Current;

        public void Dispose()
        {
            _inner.Dispose();
        }

        public Task<bool> MoveNext(CancellationToken cancellationToken)
        {
            return Task.FromResult(_inner.MoveNext());
        }
    }
}
