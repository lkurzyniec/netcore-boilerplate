using HappyCode.NetCoreBoilerplate.BooksModule.Dtos;
using HappyCode.NetCoreBoilerplate.BooksModule.IntegrationTests.Extensions;
using HappyCode.NetCoreBoilerplate.BooksModule.IntegrationTests.Infrastructure;

namespace HappyCode.NetCoreBoilerplate.BooksModule.IntegrationTests
{
    [Collection(nameof(TestServerClientCollection))]
    public class BooksTests
    {
        private readonly HttpClient _client;

        public BooksTests(TestServerClientFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public Task Get_should_return_NotFound_when_no_book()
        {
            //when
            var result = _client.GetAsync($"api/books/{int.MaxValue}", TestContext.Current.CancellationToken);

            //then
            return VerifyResultAsync(result);
        }

        [Fact]
        public Task Get_should_return_Ok_and_expected_result()
        {
            //when
            var result = _client.GetAsync("api/books/1", TestContext.Current.CancellationToken);

            //then
            return VerifyResultAsync(result);
        }

        [Fact]
        public Task Post_should_return_NoContent()
        {
            //given
            var book = new BookDto { Title = "Some_test_title" };

            //when
            var result = _client.PostAsync("api/books", book.ToStringContent(), TestContext.Current.CancellationToken);

            //then
            return VerifyResultAsync(result);
        }

        [Fact]
        public Task Delete_should_return_NoContent()
        {
            //when
            var result = _client.DeleteAsync("api/books/1", TestContext.Current.CancellationToken);

            //then
            return VerifyResultAsync(result);
        }

        [Fact]
        public Task Delete_should_return_NotFound_when_no_book()
        {
            //when
            var result = _client.DeleteAsync($"api/books/{int.MaxValue}", TestContext.Current.CancellationToken);

            //then
            return VerifyResultAsync(result);
        }

        private static Task VerifyResultAsync(Task<HttpResponseMessage> result) =>
            Verify(result).UseDirectory("Verify");
    }
}
