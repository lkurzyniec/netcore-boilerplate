using System;
using FluentAssertions;
using HappyCode.NetCoreBoilerplate.Api.Infrastructure.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace HappyCode.NetCoreBoilerplate.Api.UnitTests.Infrastructure.Filters
{
    public class HttpGlobalExceptionFilterTests
    {
        private const string _exMessage = "Some exception test message";

        private readonly HttpGlobalExceptionFilter _sut;

        private readonly Mock<IWebHostEnvironment> _envMock;

        public HttpGlobalExceptionFilterTests()
        {
            _envMock = new Mock<IWebHostEnvironment>(MockBehavior.Strict);

            _sut = new HttpGlobalExceptionFilter(_envMock.Object, new Mock<ILogger<HttpGlobalExceptionFilter>>().Object);
        }

        [Fact]
        public void When_Development_Then_exception_details_returned()
        {
            //given
            _envMock.SetupGet(x => x.EnvironmentName).Returns("Development");

            //when
            var context = GetMockedContext();
            _sut.OnException(context);

            //then
            context.Result.Should().NotBeNull();
            context.Result.Should().BeOfType<ObjectResult>()
                .Subject.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            context.Result.Should().BeOfType<ObjectResult>()
                .Subject.Value.Should().BeOfType<ErrorResponse>()
                .Subject.Exception.Should().NotBeNullOrEmpty()
                .And.Contain(_exMessage);
        }

        [Fact]
        public void When_not_Development_Then_exception_details_empty()
        {
            //given
            _envMock.SetupGet(x => x.EnvironmentName).Returns("Other");

            //when
            var context = GetMockedContext();
            _sut.OnException(context);

            //then
            context.Result.Should().NotBeNull();
            context.Result.Should().BeOfType<ObjectResult>()
                .Subject.Value.Should().BeOfType<ErrorResponse>()
                .Subject.Exception.Should().BeNullOrEmpty();
        }

        private ExceptionContext GetMockedContext()
        {
            var actionContext = new ActionContext(
                Mock.Of<HttpContext>(),
                Mock.Of<RouteData>(),
                Mock.Of<ActionDescriptor>(),
                Mock.Of<ModelStateDictionary>()
            );

            return new ExceptionContext(actionContext, [])
            {
                Exception = new Exception(_exMessage),
            };
        }
    }
}
