using System;
using System.Diagnostics;
using System.IO;  
using System.Security.Permissions;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.Configuration;

namespace FastBuild;

[Verb("link", HelpText = "Creates a Symbolic Link between the resource and development server.")]
internal class Link : IOption
{
    public async Task Run(IConfigurationRoot config)
    {
        var symLinkPath = Helper.Paths["resourcesLocal"] + new DirectoryInfo(config["resourceLocation"]).Name + Path.DirectorySeparatorChar;

        if (Directory.Exists(Helper.Paths["resourcesLocal"]))
        {
            Directory.Delete(Helper.Paths["resourcesLocal"]);
        }

        Directory.CreateDirectory(Helper.Paths["resources"]);

        await Helper.CreateSymLink(symLinkPath, config["resourceLocation"]);
    }
}