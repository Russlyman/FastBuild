using System;
using System.IO;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.Configuration;

namespace FastBuild;

internal class Program
{
    static async Task Main(string[] args)
    {
        if (!File.Exists(Helper.Paths["config"]))
        {
            Console.WriteLine("ERROR: config.json could not be found.");
            Environment.Exit(1);
        }

        if (!File.Exists(Helper.Paths["fxserverConfig"]))
        {
            Console.WriteLine("ERROR: fxserver.cfg could not be found.");
            Environment.Exit(1);
        }

        var config = new ConfigurationBuilder()
            .AddJsonFile(Helper.Paths["config"], false, false)
            .Build();

        if (string.IsNullOrWhiteSpace(config["resourcePath"]))
        {
            Console.WriteLine("ERROR: Resource path not set, check config.json.");
            Environment.Exit(1);
        }

        if (string.IsNullOrWhiteSpace(config["fxserverLicenseKey"]))
        {
            Console.WriteLine("ERROR: FXServer license key not set, check config.json.");
            Environment.Exit(1);
        }

        if (!Directory.Exists(config["resourcePath"]))
        {
            Console.WriteLine("ERROR: Resource path not valid.");
            Environment.Exit(1);
        }

        config["resourceName"] = new DirectoryInfo(config["resourcePath"]).Name;

        if (Directory.Exists(Helper.Paths["temp"]))
        {
            Directory.Delete(Helper.Paths["temp"], true);
        }

        Directory.CreateDirectory(Helper.Paths["temp"]);

        await Parser.Default.ParseArguments
            <
                SetupServer,
                StartServer,
                Build,
                UpdateServer,
                UpdateData,
                CreateSymbolicLink
            >
            (args).WithParsedAsync<IOption>(o => o.Execute(config));
    }
}