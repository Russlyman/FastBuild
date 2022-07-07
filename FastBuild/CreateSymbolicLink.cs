using System;
using System.IO;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.Configuration;

namespace FastBuild;

[Verb("link", HelpText = "Creates a symbolic link for the resource.")]
public class CreateSymbolicLink : IOption
{
    public async Task Execute(IConfigurationRoot config)
    {
        if (!Directory.Exists(Helper.Paths["fxserverData"]))
        {
            Console.WriteLine("ERROR: Server data not found, run updatedata command.");
            Environment.Exit(1);
        }

        var symLinkPath = Path.Combine(Helper.Paths["resources"], config["resourceName"]);

        // Delete FastBuild resources folder to destroy existing links.
        if (Directory.Exists(Helper.Paths["resources"]))
        {
            Directory.Delete(Helper.Paths["resources"], true);
        }

        // Create new FastBuild resources folder.
        Directory.CreateDirectory(Helper.Paths["resources"]);

        await Helper.CreateSymLink(symLinkPath, config["resourcePath"]);
    }
}