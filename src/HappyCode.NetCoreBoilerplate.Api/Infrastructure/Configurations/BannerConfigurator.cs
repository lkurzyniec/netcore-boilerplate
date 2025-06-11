using System.Diagnostics.CodeAnalysis;

namespace HappyCode.NetCoreBoilerplate.Api.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public static class BannerConfigurator
{
    private const string _cyan = "\x1b[96m";
    private const string _reverse = "\x1b[7m";
    private const string _clear = "\x1b[39m\x1b[27m";

    private const string _banner = @"{1}
            v{0}

   _   _  _____ _____   _____   _    _ ___________  ___  ______ _____ 
  | \ | ||  ___|_   _| |  _  | | |  | |  ___| ___ \/ _ \ | ___ \_   _|
  |  \| || |__   | |   | |_| | | |  | | |__ | |_/ / /_\ \| |_/ / | |  
  | . ` ||  __|  | |   \____ | | |/\| |  __|| ___ \  _  ||  __/  | |  
 _| |\  || |___  | |   .___/ / \  /\  / |___| |_/ / | | || |    _| |_ 
(_)_| \_/\____/  \_/   \____/   \/  \/\____/\____/\_| |_/\_|    \___/ 
                                                               {2}
{3}";

    public static void Print(bool withColors)
    {
        string version = Environment.GetEnvironmentVariable("HC_VERSION") ?? "local";
        string banner = withColors switch
        {
            true => string.Format(_banner, version, _cyan, _reverse, _clear),
            false => string.Format(_banner, version, null, null, null),
        };
        Console.WriteLine(banner);
    }
}
