using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using HappyCode.NetCoreBoilerplate.Api.Infrastructure.Configurations;
using HappyCode.NetCoreBoilerplate.Api.Infrastructure.Filters;
using HappyCode.NetCoreBoilerplate.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using Moq;
using Xunit;

namespace HappyCode.NetCoreBoilerplate.Api.UnitTests.Infrastructure.Filters
{
    public class ApiKeyAuthorizationFilterTests
    {
        private const string _key = "test_secret_key";

        private bool _headersAccessed = false;
        private readonly ApiKeyAuthorizationFilter _filter;

        private readonly Mock<IFeatureManager> _featureManagerMock;

        public ApiKeyAuthorizationFilterTests()
        {
            var options = Options.Create(new ApiKeySettings { SecretKey = _key });
            _featureManagerMock = new Mock<IFeatureManager>(MockBehavior.Strict);
            _featureManagerMock.Setup(x => x.IsEnabledAsync(FeatureFlags.ApiKey.ToString()))
                .ReturnsAsync(true);

            _filter = new ApiKeyAuthorizationFilter(options, _featureManagerMock.Object);
        }

        [Fact]
        public async Task When_feature_is_disabled_Then_should_immediately_returns()
        {
            //given
            _featureManagerMock.Setup(x => x.IsEnabledAsync(FeatureFlags.ApiKey.ToString()))
                .ReturnsAsync(false);

            //when
            var context = GetMockedContext(secretKey:null);
            await _filter.OnAuthorizationAsync(context);

            //then
            context.Result.Should().BeNull();
            _headersAccessed.Should().BeFalse();

            _featureManagerMock.VerifyAll();
        }

        [Fact]
        public async Task When_Authorization_header_not_presented_Then_should_return_Unauthorized()
        {
            //when
            var context = GetMockedContext(secretKey: null);
            await _filter.OnAuthorizationAsync(context);

            //then
            context.Result.Should().BeOfType<UnauthorizedObjectResult>()
                .Which.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
            _headersAccessed.Should().BeTrue();

            _featureManagerMock.VerifyAll();
        }

        [Fact]
        public async Task When_Authorization_header_is_empty_Then_should_return_Unauthorized()
        {
            //when
            var context = GetMockedContext(secretKey: string.Empty);
            await _filter.OnAuthorizationAsync(context);

            //then
            context.Result.Should().BeOfType<UnauthorizedObjectResult>()
                .Which.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
            _headersAccessed.Should().BeTrue();
        }

        [Fact]
        public async Task When_Authorization_header_has_invalid_format_Then_should_return_Unauthorized()
        {
            //when
            var context = GetMockedContext(secretKey: $"Key {_key}");
            await _filter.OnAuthorizationAsync(context);

            //then
            context.Result.Should().BeOfType<UnauthorizedObjectResult>()
                .Which.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
            _headersAccessed.Should().BeTrue();
        }

        [Fact]
        public async Task When_Authorization_header_do_not_match_Then_should_return_Unauthorized()
        {
            //when
            var context = GetMockedContext(secretKey: "APIKey ABC");
            await _filter.OnAuthorizationAsync(context);

            //then
            context.Result.Should().BeOfType<UnauthorizedObjectResult>()
                .Which.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
            _headersAccessed.Should().BeTrue();
        }

        [Fact]
        public async Task When_Authorization_header_match_Then_result_should_be_null()
        {
            //when
            var context = GetMockedContext(secretKey: $"APIKey {_key}");
            await _filter.OnAuthorizationAsync(context);

            //then
            context.Result.Should().BeNull();
            _headersAccessed.Should().BeTrue();
        }

        private AuthorizationFilterContext GetMockedContext(string secretKey)
        {
            var headers = new HeaderDictionary();
            if (secretKey != null)
            {
                headers.Add("Authorization", new[] { secretKey });
            }

            var httpRequestMock = new Mock<HttpRequest>();
            httpRequestMock.Setup(mock => mock.Headers)
                .Returns(headers)
                .Callback(() => _headersAccessed = true);

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.SetupGet(m => m.Request)
                .Returns(httpRequestMock.Object);

            var actionContext = new ActionContext(
                httpContextMock.Object,
                new Mock<RouteData>().Object,
                new Mock<ActionDescriptor>().Object
                );

            return new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>(0));
        }
    }
}
