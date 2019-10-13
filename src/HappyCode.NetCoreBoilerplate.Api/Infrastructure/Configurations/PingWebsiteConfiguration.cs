using System;

namespace HappyCode.NetCoreBoilerplate.Api.Infrastructure.Configurations
{
    public class PingWebsiteConfiguration
    {
        public Uri Url { get; set; }
        public int TimeIntervalInMinutes { get; set; }
    }
}
