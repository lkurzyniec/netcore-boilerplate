using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Http.CSharp;

namespace HappyCode.NetCoreBoilerplate.Api.LoadTests
{
    public abstract class LoadTestsBase
    {
        protected static readonly string BaseUrl;
        protected abstract string ResourceUrl { get; }

        static LoadTestsBase()
        {
            BaseUrl = Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "http://localhost:5000";
        }

        protected void ExecuteLoadTest(string action = null, string method = "GET", HttpContent body = null)
        {
            var url = $"{BaseUrl}/{ResourceUrl}{action}";
            body ??= new StringContent(string.Empty);

            var step = GetStep(url, method, body);
            var scenario = GetScenario(step);

            var stats = NBomberRunner.RegisterScenarios(new[] { scenario })
                .RunTest();

            AssertResults(stats);
        }

        private void AssertResults(NodeStats stats)
        {
            var scenarioStats = stats.ScenarioStats.First();
            scenarioStats.LatencyCount.Less800.Should().BeGreaterOrEqualTo(stats.RequestCount - 10);

            var stepStats = stats.ScenarioStats.First().StepStats.First();
            stepStats.FailCount.Should().Be(0);
            stepStats.RPS.Should().BeGreaterOrEqualTo(100);
        }

        private Scenario GetScenario(params IStep[] steps)
        {
            return ScenarioBuilder.CreateScenario($"Load test of '{ResourceUrl}'", steps)
                .WithoutWarmUp()
                .WithLoadSimulations(new[]
                {
                    Simulation.KeepConcurrentScenarios(copiesCount: 1, during: TimeSpan.FromSeconds(5)),
                    //Simulation.InjectScenariosPerSec(copiesCount: 100, during: TimeSpan.FromSeconds(10)),
                });
        }

        private IStep GetStep(string url, string method, HttpContent body)
        {
            var stepName = $"{method} '{url}'";

            return HttpStep.Create(stepName, ctx =>
                Http.CreateRequest(method, url)
                    .WithHeader("Authorization", "ApiKey ABC-xyz")
                    .WithBody(body)
                    .WithCheck(response => Task.FromResult(response.IsSuccessStatusCode))
            );
        }
    }
}
