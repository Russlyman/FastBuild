using System;
using System.IO;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Russlyman.Rcon;

namespace FastBuild;

[Verb("build", HelpText = "Perform a build.")]
public class Build : IOption
{
    [Option('p', "path", Required = true, HelpText = "DLL Path")]
    public string DllPath { get; private set; }

    public async Task Execute(IConfigurationRoot config)
    {
        var dllName = Path.GetFileName(DllPath);

        File.Copy(DllPath, Path.Combine(config["resourcePath"], dllName), true);

        int rconPort;

        try
        {
            rconPort = Convert.ToInt32(config["rconPort"]);
        }
        catch (Exception)
        {
            Console.WriteLine("Invalid port provided, check config.json.");
            Environment.Exit(1);
            return;
        }

        using var rcon = new RconClient();
        rcon.Connect(config["rconIp"], rconPort, config["rconPassword"]);
        await rcon.SendAsync($"restart { config["resourceName"] }");
    }
}