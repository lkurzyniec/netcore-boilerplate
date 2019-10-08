using Moq.AutoMock;
using HappyCode.NetCoreBoilerplate.Api.Controllers;

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

            Controller = Mocker.CreateInstance<T>();
        }
    }
}
