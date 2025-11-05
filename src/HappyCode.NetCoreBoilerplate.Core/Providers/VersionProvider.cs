using System.Collections;
using System.Linq;

namespace HappyCode.NetCoreBoilerplate.Core.Providers;

public class VersionProvider
{
    private const string _prefix = "HC_";

    private readonly Lazy<Dictionary<string, string>> _versionEntries = new(GetVersionEntries);

    private static Dictionary<string, string> GetVersionEntries()
    {
        var variables = Environment.GetEnvironmentVariables()
            .Cast<DictionaryEntry>()
            .Where(x => x.Key.ToString().StartsWith(_prefix))
            .ToDictionary(
                x => x.Key.ToString().Remove(0, _prefix.Length),
                y => y.Value.ToString());
        return variables;
    }

    public Dictionary<string, string> VersionEntries => _versionEntries.Value;
}
