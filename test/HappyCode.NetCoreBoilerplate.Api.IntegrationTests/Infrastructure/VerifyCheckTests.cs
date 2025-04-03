namespace HappyCode.NetCoreBoilerplate.Api.IntegrationTests.Infrastructure
{
    public class VerifyCheckTests
    {
        [Fact]
        public Task CheckVerify()
            => VerifyChecks.Run();
    }
}
