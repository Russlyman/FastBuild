using System;
using System.IO;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Russlyman.Rcon;

namespace FastBuild;

[Verb("run", HelpText = "Perform a build.")]
public class Run : IOption
{
    [Option('i', "input", Required = true, HelpText = "The path of the new DLL.")]
    public string Input { get; private set; }

    public async Task Execute(IConfigurationRoot config)
    {
        var dllName = Path.GetFileName(Input);

        File.Copy(Input, Path.Combine(config["resourcePath"], dllName), true);

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

        var rcon = new RconClient();
        rcon.Connect(config["rconIp"], rconPort, config["rconPassword"]);
        await rcon.SendAsync($"restart { config["resourceName"] }");
        rcon.Close();
    }
}