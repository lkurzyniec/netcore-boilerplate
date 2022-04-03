using System.Collections.Generic;
using FluentAssertions;
using HappyCode.NetCoreBoilerplate.Api.Infrastructure.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;

namespace HappyCode.NetCoreBoilerplate.Api.UnitTests.Infrastructure.Filters
{
    public class ValidateModelStateFilterTests
    {
        private readonly ValidateModelStateFilter _filter = new ValidateModelStateFilter();

        [Fact]
        public void When_ModelState_is_valid_Then_result_is_empty()
        {
            //when
            var context = GetMockedContext(null, null);
            _filter.OnActionExecuting(context);

            //then
            context.Result.Should().BeNull();
        }

        [Fact]
        public void When_ModelState_is_not_valid_Then_bad_request_returned()
        {
            //when
            var context = GetMockedContext("some_field", "Some error message");
            _filter.OnActionExecuting(context);

            //then
            context.Result.Should().NotBeNull();
            context.Result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeOfType<ErrorResponse>()
                .Which.Messages.Should().ContainMatch("*error message*");
        }

        private ActionExecutingContext GetMockedContext(string key, string errorMessage)
        {
            var modelState = new ModelStateDictionary();

            if (key != null)
            {
                modelState.AddModelError(key, errorMessage);
            }

            var actionContext = new ActionContext(
                Mock.Of<HttpContext>(),
                Mock.Of<RouteData>(),
                Mock.Of<ActionDescriptor>(),
                modelState
            );

            return new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                Mock.Of<Controller>()
            );
        }
    }
}
