using System.Net.Http;
using System.Threading.Tasks;
using HappyCode.NetCoreBoilerplate.BooksModule.Dtos;
using HappyCode.NetCoreBoilerplate.BooksModule.IntegrationTests.Extensions;
using HappyCode.NetCoreBoilerplate.BooksModule.IntegrationTests.Infrastructure;
using VerifyXunit;
using Xunit;

namespace HappyCode.NetCoreBoilerplate.Api.IntegrationTests
{
    [UsesVerify]
    [Collection(nameof(TestServerClientCollection))]
    public class BooksTests
    {
        private readonly HttpClient _client;

        public BooksTests(TestServerClientFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public Task GetAll_should_return_Ok_with_results()
        {
            //when
            var result = _client.GetAsync("api/books");

            //then
            return Verifier.Verify(result);
        }

        [Fact]
        public Task Get_should_return_NotFound_when_no_book()
        {
            //when
            var result = _client.GetAsync($"api/books/{int.MaxValue}");

            //then
            return Verifier.Verify(result);
        }

        [Fact]
        public Task Get_should_return_Ok_and_expected_result()
        {
            //when
            var result = _client.GetAsync("api/books/1");

            //then
            return Verifier.Verify(result);
        }

        [Fact]
        public Task Post_should_return_NoContent()
        {
            //given
            var book = new BookDto { Title = "Some_test_title" };

            //when
            var result = _client.PostAsync("api/books", book.ToStringContent());

            //then
            return Verifier.Verify(result);
        }

        [Fact]
        public Task Delete_should_return_NoContent()
        {
            //when
            var result = _client.DeleteAsync("api/books/1");

            //then
            return Verifier.Verify(result);
        }

        [Fact]
        public Task Delete_should_return_NotFound_when_no_book()
        {
            //when
            var result = _client.DeleteAsync($"api/books/{int.MaxValue}");

            //then
            return Verifier.Verify(result);
        }
    }
}
