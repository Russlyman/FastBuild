using System.IO;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.Configuration;

namespace FastBuild;

[Verb("link", HelpText = "Creates a Symbolic Link between the resource and development server.")]
internal class Link : IOption
{
    public async Task Run(IConfigurationRoot config)
    {
        var symLinkPath = Helper.Paths["resourcesLocal"] + new DirectoryInfo(config["resourcePath"]).Name + Path.DirectorySeparatorChar;

        if (Directory.Exists(Helper.Paths["resourcesLocal"]))
        {
            Directory.Delete(Helper.Paths["resourcesLocal"], true);
        }

        Directory.CreateDirectory(Helper.Paths["resourcesLocal"]);

        await Helper.CreateSymLink(symLinkPath, config["resourcePath"]);
    }
}