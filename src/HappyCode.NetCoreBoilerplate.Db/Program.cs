using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using DbUp;
using HappyCode.NetCoreBoilerplate.Db.Options;
using Microsoft.Extensions.Configuration;

namespace HappyCode.NetCoreBoilerplate.Db
{
    class Program
    {
        public static int Main(string[] args)
        {
            var configuration = LoadAppConfiguration();
            var upgradeOptions = configuration.GetSection("UpgradeOptions").Get<UpgradeOptions>();
            var scriptsAndCodePatternPattern = new Regex(upgradeOptions.ScriptsAndCodePattern ?? ".*", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            var upgrader = DeployChanges.To
                .SqlDatabase(configuration.GetConnectionString("MsSqlDb"))
                .WithScriptsAndCodeEmbeddedInAssembly(Assembly.GetExecutingAssembly(), (fileName) => scriptsAndCodePatternPattern.IsMatch(fileName))
                .WithExecutionTimeout(TimeSpan.FromSeconds(upgradeOptions.CommandExecutionTimeoutSeconds))
                .WithTransaction()
                .LogToConsole()
                .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            return 0;
        }

        private static IConfigurationRoot LoadAppConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
