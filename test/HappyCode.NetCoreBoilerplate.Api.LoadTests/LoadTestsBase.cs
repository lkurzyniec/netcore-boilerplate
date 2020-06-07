using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
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
            body ??= new StringContent(string.Empty);

            var step = CreateStep(action, method, body);
            var scenario = CreateScenario(step);

            var stats = NBomberRunner.RegisterScenarios(new[] { scenario })
                .RunTest();

            AssertResults(stats);
        }

        protected void ExecuteLoadTests(params IStep[] steps)
        {
            var scenario = CreateScenario(steps);

            var stats = NBomberRunner.RegisterScenarios(new[] { scenario })
                .RunTest();

            AssertResults(stats);
        }

        protected IStep CreateStep(string action, string method, HttpContent body)
        {
            var url = $"{BaseUrl}/{ResourceUrl}{action}";
            var stepName = $"{method} '{url}'";

            return HttpStep.Create(stepName, ctx =>
                Http.CreateRequest(method, url)
                    .WithHeader("Authorization", "ApiKey ABC-xyz")
                    .WithBody(body)
                    .WithCheck(async response =>
                    {
                        Trace.WriteLine($"Response status: {response.StatusCode}");
                        if (!response.IsSuccessStatusCode)
                        {
                            Trace.TraceError(await response.Content.ReadAsStringAsync());
                        }
                        return response.IsSuccessStatusCode;
                    })
            );
        }

        private void AssertResults(NodeStats stats)
        {
            var scenarioStats = stats.ScenarioStats.First();
            scenarioStats.LatencyCount.Less800.Should().BeGreaterOrEqualTo(stats.RequestCount - 10);

            scenarioStats.StepStats.ToList().ForEach(stepStats =>
            {
                stepStats.FailCount.Should().Be(0);
                stepStats.RPS.Should().BeGreaterOrEqualTo(20);
            });
        }

        private Scenario CreateScenario(params IStep[] steps)
        {
            return ScenarioBuilder.CreateScenario($"Load test of '{ResourceUrl}'", steps)
                .WithoutWarmUp()
                .WithLoadSimulations(new[]
                {
                    Simulation.KeepConcurrentScenarios(copiesCount: 1, during: TimeSpan.FromSeconds(5)),
                    //Simulation.InjectScenariosPerSec(copiesCount: 100, during: TimeSpan.FromSeconds(10)),
                });
        }
    }
}
