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
        private static readonly Regex _defaultRunPattern = new Regex(@".*", RegexOptions.Compiled);

        public static int Main(string[] args)
        {
            var configuration = LoadAppConfiguration();
            var upgradeOptions = configuration.GetSection("UpgradeOptions").Get<UpgradeOptions>();
            var runPattern = TryGetRunPattern(upgradeOptions.RunPattern) ?? _defaultRunPattern;

            var upgrader = DeployChanges.To
                .SqlDatabase(configuration.GetConnectionString("MsSqlDb"))
                .WithScriptsAndCodeEmbeddedInAssembly(Assembly.GetExecutingAssembly(), (fileName) => runPattern.IsMatch(fileName))
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

        private static Regex TryGetRunPattern(string candidateRegeix)
            => string.IsNullOrWhiteSpace(candidateRegeix) ? null : new Regex(candidateRegeix);

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
