using System.Diagnostics;
using HappyCode.NetCoreBoilerplate.Core.Providers;
using Serilog.Core;
using Serilog.Events;

namespace HappyCode.NetCoreBoilerplate.Api.Infrastructure.Logging;

[DebuggerStepThrough]
public class VersionEnricher : ILogEventEnricher
{
    private readonly VersionProvider _versionProvider;

    public VersionEnricher(VersionProvider versionProvider)
    {
        _versionProvider = versionProvider;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        foreach (var item in _versionProvider.VersionEntries)
        {
            logEvent.AddPropertyIfAbsent(new LogEventProperty(item.Key, new ScalarValue(item.Value)));
        }
    }
}
