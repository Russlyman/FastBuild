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
        var symLinkPath = Helper.Paths["resources"] + new DirectoryInfo(config["resourceLocation"]).Name + Path.DirectorySeparatorChar;

        if (Directory.Exists(symLinkPath))
        {
            Directory.Delete(symLinkPath);
        }

        await Helper.CreateSymLink(symLinkPath, config["resourceLocation"]);
    }
}