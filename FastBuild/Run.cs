using System;
using System.IO;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Russlyman.Rcon;

namespace FastBuild;

[Verb("run", HelpText = "Runs FastBuild.")]
public class Run : IOption
{
    [Option('i', "input", Required = true, HelpText = "The path of the new DLL.")]
    public string Input { get; private set; }

    public async Task Execute(IConfigurationRoot config)
    {
        var dllName = Path.GetFileName(Input);

        File.Copy(Input, Path.Combine(config["resourcePath"], dllName), true);

        var resourceName = new DirectoryInfo(config["resourcePath"]).Name;

        var rcon = new RconClient();
        rcon.Connect("127.0.0.1", 30130, "fastbuild");
        await rcon.SendAsync($"restart { resourceName }");
        rcon.Close();
    }
}