namespace HappyCode.NetCoreBoilerplate.Api.Infrastructure.Configurations;

public static class BannerConfigurator
{
    private const string _cyan = "\x1b[96m";
    private const string _reverse = "\x1b[7m";
    private const string _clear = "\x1b[39m\x1b[27m";

    private const string _banner = @"{0}
            _                         _           _ _                 _       _
 _ __   ___| |_ ___ ___  _ __ ___    | |__   ___ (_) | ___ _ __ _ __ | | __ _| |_ ___
| '_ \ / _ \ __/ __/ _ \| '__/ _ \___| '_ \ / _ \| | |/ _ \ '__| '_ \| |/ _` | __/ _ \
| | | |  __/ || (_| (_) | | |  __/___| |_) | (_) | | |  __/ |  | |_) | | (_| | ||  __/
|_| |_|\___|\__\___\___/|_|  \___|   |_.__/ \___/|_|_|\___|_|  | .__/|_|\__,_|\__\___|
                                                               |_| {1}by Łukasz Kurzyniec
{2}";

    public static void Print(bool withColors)
    {
        string banner = withColors switch
        {
            true => string.Format(_banner, _cyan, _reverse, _clear),
            false => string.Format(_banner, null, null, null),
        };
        Console.WriteLine(banner);
    }
}
