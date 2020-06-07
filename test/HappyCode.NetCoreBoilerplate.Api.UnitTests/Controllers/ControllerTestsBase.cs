using HappyCode.NetCoreBoilerplate.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Moq.AutoMock;

namespace HappyCode.NetCoreBoilerplate.Api.UnitTests.Controllers
{
    public abstract class ControllerTestsBase<T>
        where T : ApiControllerBase
    {
        protected readonly T Controller;
        protected readonly AutoMocker Mocker;

        protected ControllerTestsBase()
        {
            Mocker = new AutoMocker();

            var httpResponseMock = Mocker.GetMock<HttpResponse>();
            httpResponseMock.Setup(mock => mock.Headers).Returns(new HeaderDictionary());

            var httpRequestMock = Mocker.GetMock<HttpRequest>();

            var httpContextMock = Mocker.GetMock<HttpContext>();
            httpContextMock.Setup(mock => mock.Response).Returns(httpResponseMock.Object);
            httpContextMock.Setup(mock => mock.Request).Returns(httpRequestMock.Object);

            Controller = Mocker.CreateInstance<T>();
            Controller.ControllerContext.HttpContext = httpContextMock.Object;
        }
    }
}
