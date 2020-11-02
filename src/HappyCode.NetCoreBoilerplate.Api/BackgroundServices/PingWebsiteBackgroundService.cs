using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HappyCode.NetCoreBoilerplate.Core.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HappyCode.NetCoreBoilerplate.Api.BackgroundServices
{
    public class PingWebsiteBackgroundService : BackgroundService
    {
        private readonly HttpClient _client;
        private readonly ILogger<PingWebsiteBackgroundService> _logger;
        private readonly IOptions<PingWebsiteSettings> _configuration;

        public PingWebsiteBackgroundService(
            IHttpClientFactory httpClientFactory,
            ILogger<PingWebsiteBackgroundService> logger,
            IOptions<PingWebsiteSettings> configuration)
        {
            _client = httpClientFactory.CreateClient(nameof(PingWebsiteBackgroundService));
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(PingWebsiteBackgroundService)} running at '{DateTime.Now}', pinging '{_configuration.Value.Url}'");
                try
                {
                    using var response = await _client.GetAsync(_configuration.Value.Url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
                    _logger.LogInformation($"Is '{_configuration.Value.Url.Authority}' responding: {response.IsSuccessStatusCode}");
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error during ping");
                }
                await Task.Delay(TimeSpan.FromMinutes(_configuration.Value.TimeIntervalInMinutes), cancellationToken);
            }
        }
    }
}
