using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Russlyman.Rcon;

namespace FastBuild;

[Verb("build", HelpText = "Performs a build.")]
public class Build : IOption
{
    [Option('p', "path", Required = true, HelpText = "DLL Path")]
    public string DllPath { get; private set; }

    public async Task Execute(IConfigurationRoot config)
    {
        // Check if user provided DLL path exists.
        if (!File.Exists(DllPath))
        {
            Console.WriteLine("ERROR: DLL file does not exist.");
            Environment.Exit(1);
        }

        var dllName = Path.GetFileName(DllPath);

        File.Copy(DllPath, Path.Combine(config["resourcePath"], dllName), true);

        // Convert rconPort config entry to integer.
        int rconPort;

        try
        {
            rconPort = Convert.ToInt32(config["rconPort"]);
        }
        catch (Exception)
        {
            Console.WriteLine("ERROR: Invalid port, check config.json.");
            Environment.Exit(1);
            return;
        }

        using var rcon = new RconClient();

        try
        {
            rcon.Connect(config["rconIp"], rconPort, config["rconPassword"]);
        }
        catch (Exception e)
        {
            switch (e)
            {
                case FormatException:
                    Console.WriteLine("ERROR: Invalid RCon IP or RCon port, check config.json.");
                    break;
                case ArgumentException:
                    Console.WriteLine("ERROR: Invalid RCon password, check config.json.");
                    break;
                default:
                    throw;
            }

            Environment.Exit(1);
        }

        try
        {
            await rcon.SendAsync($"restart { config["resourceName"] }");
        }
        catch (SocketException)
        {
            Console.WriteLine("ERROR: Development server unreachable, ensure server is started.");
            Environment.Exit(1);
        }
    }
}