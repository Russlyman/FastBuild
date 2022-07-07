using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.Configuration;

namespace FastBuild;

[Verb("startserver", HelpText = "Starts FXServer development server.")]
public class StartServer : IOption
{
    public async Task Execute(IConfigurationRoot config)
    {
        if (!Directory.Exists(Helper.Paths["fxserverData"]) || !Directory.Exists(Helper.Paths["fxserver"]))
        {
            Console.WriteLine("ERROR: Development server not setup, run setup command.");
            Environment.Exit(1);
        }

        if (!File.Exists(Helper.Paths["fxserverConfig"]))
        {
            Console.WriteLine("ERROR: fxserver.cfg could not be found.");
            Environment.Exit(1);
        }

        if (string.IsNullOrWhiteSpace(config["fxserverLicenseKey"]))
        {
            Console.WriteLine("ERROR: FXServer license key not set, check config.json.");
            Environment.Exit(1);
        }

        if (!Directory.Exists(Path.Combine(Helper.Paths["resources"], config["resourceName"])))
        {
            Console.WriteLine("ERROR: Symbolic link not found, run link command.");
            Environment.Exit(1);
        }

        var server = new ProcessStartInfo
        {
            WorkingDirectory = Helper.Paths["fxserverData"],
            FileName = Helper.Paths["fxserverBinary"],
            Arguments = $"+set sv_licenseKey {config["fxserverLicenseKey"]} +exec \"{ Helper.Paths["fxserverConfig"] }\" +ensure { config["resourceName"] }"
        };

        using var process = Process.Start(server);
        await process.WaitForExitAsync();
    }
}