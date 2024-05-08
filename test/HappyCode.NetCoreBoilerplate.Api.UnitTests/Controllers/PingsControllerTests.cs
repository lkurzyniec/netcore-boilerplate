using System.Net;
using AutoFixture.Xunit2;
using FluentAssertions;
using HappyCode.NetCoreBoilerplate.Api.BackgroundServices;
using HappyCode.NetCoreBoilerplate.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace HappyCode.NetCoreBoilerplate.Api.UnitTests.Controllers
{
    public class PingsControllerTests : ControllerTestsBase<PingsController>
    {
        [Theory, AutoData]
        public void GetWebsitePingStatusCode_should_return_Ok_with_expected_result(HttpStatusCode code)
        {
            //given
            var pingServiceMock = Mocker.GetMock<IPingService>();
            pingServiceMock.SetupGet(x => x.WebsiteStatusCode)
                .Returns(code);

            //when
            var result = Controller.GetWebsitePingStatusCode() as OkObjectResult;

            //then
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeAssignableTo<string>();
            var value = result.Value as string;
            value.Should().Contain(code.ToString());
            value.Should().Contain(((int)code).ToString());
        }

        [Fact]
        public void GetRandomStatusCode_should_return_Ok_with_expected_result()
        {
            //when
            var result = Controller.GetRandomStatusCode() as OkObjectResult;

            //then
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeAssignableTo<string>();
            var value = result.Value as string;
            value.Should().NotBeNullOrEmpty();
        }
    }
}
